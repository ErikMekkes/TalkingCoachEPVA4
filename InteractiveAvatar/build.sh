#!/bin/bash
# CONFIGURE YOUR SYSTEM
VERSION="0.2.0"
UNITY_INSTALL="C:\Program Files\Unity\Editor\Unity.exe"

# CONFIGURE YOUR SYSTEM ABOVE THIS LINE
# --------------------------
# DON'T EDIT BELOW THIS LINE

POSITIONAL=()
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
	-h|--help)
    SHOW_HELP=TRUE
    shift # shift past the argument
    ;;
    -r|--run)
    RUN_DEV_SERVER=TRUE
    shift # shift past the argument
    ;;
    *)    # case unknown option / positional option
    POSITIONAL+=("$1") # save it in an array for later
    shift # past argument
    ;;
esac
done
set -- "${POSITIONAL[@]}" # restore positional parameters

if [ "${SHOW_HELP}" = "TRUE" ]; then
	echo "usage: ./build.sh [options]"
	echo "Options and arguments:"
	echo "-h --help  : Show this help dialogue"
	echo "-r --run   : After building, start a webserver"
else
	echo "Building project InteractiveAvatar ${VERSION}"

	echo UNITY_INSTALL   = "${UNITY_INSTALL}"
	echo RUN_DEV_SERVER  = "${RUN_DEV_SERVER}"

	ARGS=""

	if [ "${RUN_DEV_SERVER}" = "TRUE" ];
	then
		ARGS+="--ia-run "
	fi

	echo "Building with command"
	echo "${UNITY_INSTALL} -quit -batchmode -executeMethod ScriptBatch.BuildGame ${ARGS}"

	echo 
	echo 
	echo "Building now"

	"${UNITY_INSTALL}" -quit -batchmode -executeMethod ScriptBatch.BuildGame "${ARGS}"

	echo
	echo "Build finished"
	pause
fi