var path = require('path');

module.exports = {
    entry: './src/ma-send-message-azure-queue.plugin.ts',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: path.resolve(__dirname, "node_modules")
            }
        ]
    },
    resolve: {
        extensions: [".ts", ".js"]
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: 'send_message_azure_queue.plugin.js',
        library: "publishActivities",
        libraryTarget: "umd"
    },
    devtool: 'source-map',
    externals: [
        "@sitecore/ma-core",
        "@angular/core",
        "@ngx-translate/core"
    ]
};