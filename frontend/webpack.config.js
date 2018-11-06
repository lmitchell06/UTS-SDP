'use strict';

var webpack = require('webpack');
var path = require('path');
var exec = require('executive');
var fs = require('async-file');
var humanSize = require('human-size');

var ExtractTextPlugin = require('extract-text-webpack-plugin')
var MinifyPlugin = require('babel-minify-webpack-plugin');
var WebpackOnBuildPlugin = require('on-build-webpack');

var babelOptions = {
    presets: [
        [
            "env",
            {
                modules: false,
                targets: { browsers: "last 3 versions" },
                exclude: [ "transform-regenerator" ]
            }
        ]
    ],
    plugins: [
        ["babel-plugin-transform-async-to-module-method", {
            module: "co",
            method: "wrap"
        }],
        'jsx-v-model',
        'transform-vue-jsx'
    ]
};

var plugins = [
    new webpack.ProvidePlugin({
        Promise: 'promise-polyfill'
    }),
    new ExtractTextPlugin('build.css'),
    new webpack.ContextReplacementPlugin(/moment[\/\\]locale$/, /en-au/)
];
var cssPlugins = [
    'css-loader',
    'sass-loader'
];

if(process.env.NODE_ENV && process.env.NODE_ENV.startsWith('p')) {

    // minify and zopfli js (zopfli is just to see final gzipped size)
    plugins.push(
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new MinifyPlugin(),
        new WebpackOnBuildPlugin(async stats => {
            try {
                await exec(`bash -c "zopfli dist/build.js"`)
                await exec(`bash -c "zopfli dist/build.css"`)
                var inputjs = await fs.stat('dist/build.js');
                var outputjs = await fs.stat('dist/build.js.gz');
                var inputcss = await fs.stat('dist/build.css');
                var outputcss = await fs.stat('dist/build.css.gz');
                var percentjs = Math.round((outputjs.size / inputjs.size) * 100);
                var percentcss = Math.round((outputcss.size / inputcss.size) * 100);
                console.log(`Compressed build.js using zopfli.\t${humanSize(inputjs.size)} => ${humanSize(outputjs.size)} (${percentjs}%)`);
                console.log(`Compressed build.css using zopfli.\t${humanSize(inputcss.size)} => ${humanSize(outputcss.size)} (${percentcss}%)`);
            } catch(e) {
                console.error(e);
            }
        })
    );

    // minify css
    cssPlugins.splice(1, 0, 'clean-css-loader')
} else {
    plugins.push(new webpack.DefinePlugin({
        'process.env': {
            NODE_ENV: '"development"'
        }
    }));
}



var web = {
    entry: {
        main: './src/app.tsx'
    },
    output: {
        path: path.join(__dirname, 'dist'),
        filename: 'build.js',
    },
    plugins: plugins,
    cache: true,
    module: {
        rules: [{
            test: /\.tsx?$/,
            exclude: /node_modules/,
            use: [{
                loader: 'babel-loader',
                options: babelOptions
            },
            {
                loader: 'ts-loader'
            }]
        }, {
            test: /\.js$/,
            exclude: /node_modules/,
            use: [{
                loader: 'babel-loader',
                options: babelOptions
            }]
        }, {
            test: /\.s?css$/,
            use: ExtractTextPlugin.extract({ use: cssPlugins })
        },  {
            test: /\.(jpg|png|svg)$/,
            use: [{
                loader: 'file-loader',
                options: {
                    name: 'assets/[name].[ext]'
                }
            }]
        }]
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.css', '.scss']
    },
    target: 'web',
    devtool: 'source-map'
}

module.exports = web;