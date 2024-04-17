$name = "seqH6";

$isRunning = 0;
docker run --name $name -d --restart unless-stopped -e ACCEPT_EULA=Y -p 81:80 -p 5342:5341 datalust/seq:2024

while($isRunning -eq 0)
{
    $list = (docker ps -f "name=$name" -f "status=running")
    if($list[1].Contains($name))
    {
        $isRunning = 1;
        $isRunning
    }
}

Start-Sleep -Seconds 10 # just to be sure it is ready for connections.
$ip = docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $name
$ip
docker run --rm datalust/seqcli:latest apikey create -t cateringLTP --token 12345678901234567890 --property=ProgramSource=DevCateringLTP -s http://$ip
docker run --rm datalust/seqcli:latest apikey create -t userAPI --token 22345678901234567890 --property=ProgramSource=DevUserAPI -s http://$ip