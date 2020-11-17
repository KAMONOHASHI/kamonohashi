module.exports = ({ config }) => {
  config.module.rules.push({
    test: /\.scss$/,
    loaders: ['style-loader', 'css-loader', 'sass-loader'],
  })
  return config
}
