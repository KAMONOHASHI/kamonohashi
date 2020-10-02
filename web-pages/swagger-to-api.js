/* eslint-disable */
const swaggerGen = require('swagger-vue')
const jsonData = require('../swagger.json') // モノリポジトリ直下のswagger.jsonを参照
const fs = require('fs')
const path = require('path')
const readline = require('readline')

let tmpfile = path.join(__dirname, './temp.js')
let outfile = path.join(__dirname, './src/api/v1/api.generator.js')

// 自動生成で api.generator.js を作成
let opt = {
  swagger: jsonData,
  moduleName: 'api',
  className: 'api',
}
const codeResult = swaggerGen(opt)
fs.writeFileSync(tmpfile, codeResult)

// api.generator.js をカスタマイズ
let stream = fs.createReadStream(tmpfile)
let reader = readline.createInterface({ input: stream })
let data = ''
let skip = false
let fix1 = false
let fix2 = false
let fix3 = false
reader
  .on('line', line => {
    if ("import axios from 'axios'" === line) {
      console.log('found fix1')
      fix1 = true
      data += "import net from 'axios'\n"
      return
    }

    if ("let domain = ''" === line) {
      console.log('found fix2')
      fix2 = true
      data += line + '\n'
      data += 'let axios = net.create()\n'
      data += 'export const getAxios = () => {\n'
      data += '  return axios\n'
      data += '}\n'
      data += 'export const setAxios = ($axios) => {\n'
      data += '  axios = $axios\n'
      data += '}\n'
      return
    }

    if ('    return axios[method](queryUrl, {' === line) {
      skip = true
    }
    if (skip && '  } else {' === line) {
      skip = false

      console.log('found fix3')
      fix3 = true
      data += '    let p = config || {}\n'
      data += '    p.params = form\n'
      data += '    return axios[method](queryUrl, p)\n'
    }

    if (!skip) {
      data += line + '\n'
    }
  })
  .on('close', () => {
    if (fix1 && fix2 && fix3) {
      fs.writeFileSync(outfile, data)
      fs.unlinkSync(tmpfile)
    } else {
      if (fix1 === false) console.error('not found fix1')
      if (fix2 === false) console.error('not found fix2')
      if (fix3 === false) console.error('not found fix3')
    }
  })
