import os
import sys
import time
import subprocess
import shutil
from os.path import join
from sys import stderr
import subprocess
import argparse

def main():
    # Current path should be a checkout directory
    cur_path = os.getcwd()
    sc_bin_path = join(cur_path, "sc", "level1", "bin", "release")
    ks_path = join(cur_path, "KitchenSink")
    srv_path = join(cur_path, "starcounter-workdir")

    prepare_starcounter(sc_bin_path)
    create_starcounter_repo(srv_path, sc_bin_path)
    build_and_run(ks_path, sc_bin_path)
    kill_starcounter()
    print("Build finished")


def print_err(msg):
    print(msg, file=stderr)


def run_check(cmd, *args, **kwargs):
    subprocess.run(cmd, check=True, *args, **kwargs)


def prepare_starcounter(sc_bin_path):
    if not os.path.exists(sc_bin_path):
        print_err('ERROR: Directory "{0}" containing Starcounter binaries does not exist'.format(sc_bin_path))
        sys.exit(1)

    print("Adding Starcounter to path")
    os.environ["StarcounterBin"] = sc_bin_path
    os.environ["Path"] = os.environ["Path"] + ";" + sc_bin_path

    run_check(["staradmin", "kill", "all"])

def create_starcounter_repo(srv_path, sc_bin_path):
    if os.path.exists(srv_path):
        print("Removing starcounter repo dir '{0}'".format(srv_path))
        shutil.rmtree(srv_path)

    run_check(['star', '@@CreateRepo', '{0}'.format(srv_path)])

    # Saving personal.xml file(TODO: add to CreateRepo)
    personal_xml_path = sc_bin_path + "/configuration/personal.xml"
    if not os.path.exists(personal_xml_path):
        print_err("WARNING: Personal XML file does not exist", file=stderr)

    with open(personal_xml_path, "w") as personal_xml_file:
        personal_xml_file.write('<?xml version="1.0" encoding="UTF-8"?>')
        personal_xml_file.write("<service><server-dir>{0}</server-dir></service>".format(srv_path + "/personal"))

    # Starting some Starcounter process
    print_err("Starting async Starcounter processes")
    sc_proc = subprocess.Popen(["scservice"])

def build_and_run(ks_path, sc_bin_path):
    msbuild_path = 'C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\msbuild.exe'
    run_check([join(ks_path, "nuget.exe"), 'restore', ks_path])
    for csproj_path in [join(ks_path, "src/KitchenSink/KitchenSink.csproj"),
                        join(ks_path, "test/KitchenSink.Tests/KitchenSink.Tests.csproj")]:
        run_check('{0} /t:clean;build /p:ReferencePath="{1};{1}\\EditionLibraries" {2}'.format(
            msbuild_path, sc_bin_path, csproj_path))

    run_check(["star",
        "-d=kitchensink",
        "--resourcedir={0}\\src\\KitchenSink\\wwwroot".format(ks_path),
        "{0}\\bin\\Debug\\KitchenSink.exe".format(ks_path)])
    run_check([join(ks_path, "packages\\NUnit.ConsoleRunner.3.2.0\\tools\\nunit3-console.exe"),
        join(ks_path, "test\\KitchenSink.Tests\\KitchenSink.Tests.csproj"),
        "/config:Debug",
        "--teamcity"])


def kill_starcounter():
    print("Killing Starcounter processes...")
    run_check(["staradmin", "kill", "all"])


if __name__ == "__main__":
    main()
