import os
import sys
import time
import subprocess
import shutil
from os.path import join, abspath
from sys import stderr
import subprocess
import argparse
import glob

def main():
    cur_path = os.getcwd()
    def_srv_path = join(cur_path, "starcounter-workdir")

    parser = argparse.ArgumentParser()
    parser.add_argument("-s", "--starcounter-bin", help="path containing starcounter binaries", required=True)
    parser.add_argument("-p", "--solution-dir", help="path to solution", required=True)
    parser.add_argument("--skip-tests", help="skip tests build and execution", action="store_true")
    parser.add_argument("-w", "--starcounter-work-dir", help="path where starcounter work files will reside. Will be cleaned!", default=def_srv_path)
    parser.add_argument("--msbuild", help="path to msbuild executable", default='C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\msbuild.exe')
    parser.add_argument("--nuget", help="path nuget executable")
    parser.add_argument("--nunit-args", action='append', help="additional args passed to nunit", default=[])

    args = parser.parse_args()

    if not args.nuget:
        args.nuget = join(args.solution_dir, "tools", "nuget.exe")

    prepare_starcounter(args.starcounter_bin)
    create_starcounter_repo(args.starcounter_work_dir, args.starcounter_bin)
    build_and_run(args.solution_dir,
        abspath(args.starcounter_bin), # intermittent problems with msbuild occur without abs here
        args.msbuild,
        args.nuget,
        args.nunit_args,
        args.skip_tests)
    kill_starcounter()
    print("Build finished")


def print_err(msg):
    print(msg, file=stderr)


def run_check(cmd, *args, **kwargs):
    if isinstance(cmd, str):
        # can't reliably distinguish executable (spaces in path)
        print("executing: "+cmd)
    elif not shutil.which(cmd[0]):
        raise IOError("Could not find executable: {0}".format(cmd[0]))

    subprocess.run(cmd, check=True, *args, **kwargs)


def prepare_starcounter(sc_bin_path):
    if not os.path.exists(sc_bin_path):
        print_err('ERROR: Directory "{0}" containing Starcounter binaries does not exist'.format(sc_bin_path))
        sys.exit(1)

    # adding relative path to env would be silly
    abs_sc_bin_path = abspath(sc_bin_path)
    print("Adding Starcounter to path")
    os.environ["StarcounterBin"] = abs_sc_bin_path
    os.environ["Path"] = os.environ["Path"] + ";" + abs_sc_bin_path

    run_check(["staradmin", "kill", "all"])

def create_starcounter_repo(srv_path, sc_bin_path):
    if os.path.exists(srv_path):
        print("Removing starcounter repo dir '{0}'".format(srv_path))
        shutil.rmtree(srv_path)

    run_check(['star', '@@CreateRepo', '{0}'.format(srv_path)])

    # Saving personal.xml file(TODO: add to CreateRepo)
    personal_xml_path = sc_bin_path + "/configuration/personal.xml"
    if not os.path.exists(personal_xml_path):
        print_err("WARNING: Personal XML file does not exist")

    with open(personal_xml_path, "w") as personal_xml_file:
        personal_xml_file.write('<?xml version="1.0" encoding="UTF-8"?>')
        personal_xml_file.write("<service><server-dir>{0}</server-dir></service>".format(srv_path + "/personal"))

    # Starting some Starcounter process
    print_err("Starting async Starcounter processes")
    sc_proc = subprocess.Popen(["scservice"])

def build_and_run(ks_path, sc_bin_path, msbuild_path, nuget_path, nunit_args, skip_tests):
    run_check([nuget_path, 'restore', ks_path])
    src_paths = glob.glob(join(ks_path, "src/*/*.csproj"))
    test_paths = glob.glob(join(ks_path, "test/*/*.csproj")) if not skip_tests else []
    for csproj_path in src_paths + test_paths:
        # using single string because of quoting issues when sc_bin_path contains spaces
        # if you can get it to run with list instead, go ahead and change it
        run_check('"{0}" /t:clean;build /p:ReferencePath="{1};{1}\\EditionLibraries" {2}'.format(
            msbuild_path, sc_bin_path, csproj_path))

    run_check(["star",
        "-d=kitchensink",
        "--resourcedir={0}\\src\\KitchenSink\\wwwroot".format(ks_path),
        "{0}\\bin\\Debug\\KitchenSink.exe".format(ks_path)])
    if not skip_tests:
        run_check([join(ks_path, "packages\\NUnit.ConsoleRunner.3.2.0\\tools\\nunit3-console.exe"),
            join(ks_path, "test\\KitchenSink.Tests\\KitchenSink.Tests.csproj"),
            "/config:Debug",
            "--teamcity"] + nunit_args)


def kill_starcounter():
    print("Killing Starcounter processes...")
    run_check(["staradmin", "kill", "all"])


if __name__ == "__main__":
    main()
