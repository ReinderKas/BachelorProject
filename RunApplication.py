import subprocess
import sys


# Update npm packages if we give any argument to the project.
if (len(sys.argv) == 2 and sys.argv[1] == "--build"):
    print("Updating npm packages.")
    subprocess.run("npm i", shell=True, check=True, cwd=r".\\Code\\DiagnosticsFrontEnd")
    print("\033[32m" + "\nFront end packages are up to date." + "\033[0m")

print("Starting application in Docker...")
try:
    subprocess.run(["docker", "compose", "up"] + sys.argv[1:], shell=True, check=True, cwd=r".\\Code")
except:
    print("\nMake sure that Docker is running before starting the project!")