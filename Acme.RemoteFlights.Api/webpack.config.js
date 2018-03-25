const path = require('path');
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const precss = require('precss');
const autoprefixer = require('autoprefixer');

module.exports = {
  entry: path.resolve(__dirname, 'app/index'),
  devServer: {
    historyApiFallback: true,
    outputPath: path.join(__dirname, 'wwwroot'),
  },
  resolve: {
    extensions: ['', '.js', '.jsx'],
  },
  output: {
    path: path.resolve(__dirname, 'wwwroot/js'),
    publicPath: '/js/',
    filename: 'bundle.js',
  },
  plugins: [
      new CleanWebpackPlugin(['wwwroot']),
      new webpack.optimize.UglifyJsPlugin({
          minimize: true,
          compress: false,
      }),
      new CopyWebpackPlugin([
          {
              context: path.resolve(__dirname, 'content/css'),
              from: '**/*',
              to: path.resolve(__dirname, 'wwwroot/css'),
          },
      ]),
  ],
  debug: true,
  devtool: '#eval-source-map',
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /(node_modules|bower_components)/,
        loaders: ['babel'],
        },
        {
            test: /\.css$/,
            loader: 'style-loader!css-loader!postcss-loader',
        },
    ],
  },
  postcss: () => [
      precss,
      autoprefixer,
  ],
};
