param(
	[string]$appsettings="..\TelegramBotServer\appsettings.json"
)

$ngrok_path = ".\ngrok.exe"
$ngrok_config_path = ".\ngrok.yml"
$ngrok_port = "4045" # Must be change when change ngrok.yml
$application_port = "8443"


if (-Not (Test-Path $ngrok_path -PathType Leaf)) {
	"Make sure that ngrok store by path '$ngrok_path'"
	exit
}
if (-Not (Test-Path $ngrok_config_path -PathType Leaf)) {
	"Make sure that ngrok.yml located by path '$ngrok_config_path'"
	exit
}
if (-Not (Test-Path $appsettings -PathType Leaf)) {
	"Make sure that appsettings.json located by path '$appsettings'"
	"Or pass path as script first argument"
	exit
}

Start-Process $ngrok_path -ArgumentList "http", $application_port, "--config", $ngrok_config_path

$response = (New-Object System.Net.WebClient).DownloadString("http://localhost:4045/api/tunnels") | ConvertFrom-Json

$file = (Get-Content $appsettings -Raw) 

$web_hook = $response.Tunnels.public_url

$file = $file -replace "`"WebHookURL`": `".*`"",  "`"WebHookURL`": `"$web_hook`""

$file  *> "$appsettings"
