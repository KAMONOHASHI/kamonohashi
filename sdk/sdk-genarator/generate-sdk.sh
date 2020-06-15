#!/usr/bin/env bash
#
#  Generate part of kamonohashi-sdk

codegen_jars=($(ls -v swagger-codegen-cli-*.jar 2> /dev/null))
if [[ ${#codegen_jars[@]} -eq 0 ]]; then
  echo "no codegen jar."
  exit 1
fi
codegen_jar="${codegen_jars[-1]}"

shopt -s -o errexit

package=kamonohashi.op.rest
package_dir="${package//./\/}"
gen_opt=(
  --git-repo-id kamonohashi
  --git-user-id KAMONOHASHI
  --lang python
  -DpackageName="${package}"
  -DprojectName=kamonohashi-sdk
  -DpackageVersion=
  -DpackageUrl=https://github.com/KAMONOHASHI/kamonohashi
  -DapiTests=false
  -DmodelTests=false
)
codegen_log=codegen.log

tempdir="$(mktemp -d)"
trap 'rm -rf "${tempdir}"' EXIT

generate() {
  local top filtered_spec
  top="$1"
  filtered_spec="$(mktemp --suffix=.json -p "${tempdir}")"
  shift
  python filter-sdk.py "$@" < swagger.json > "${filtered_spec}"
  rm -rf "${top}"
  java -jar "${codegen_jar}" generate --input-spec "${filtered_spec}" --output "${top}" "${gen_opt[@]}" &>> "${codegen_log}"
  (cd "${top}/${package}" && cp -pr . "../${package_dir}")
  rm -rf "${top:?}/${package}"
  patch -u -p0 -d "${top}" -s < rest.py.patch
}

rm -f "${codegen_log}"
generate output
generate output.with_comment --preserve-comment
