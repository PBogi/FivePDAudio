fx_version 'adamant'
games { 'gta5' }

author 'Bogi'
description 'FivePD Audio Expansion'
version '0.1.0'

client_script 'FivePDAudio_Client.net.dll'

ui_page('AUDIO/index.html')

files {
    'Newtonsoft.Json.dll',
    'AUDIO/index.html',
    'AUDIO/howler.min.js',
    'AUDIO/**/*'
}