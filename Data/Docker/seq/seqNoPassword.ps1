$name = "seqH6";

$isRunning = 0;
docker run --name $name -d --restart unless-stopped -e ACCEPT_EULA=Y -p 81:80 -p 5341:5341 datalust/seq:2024

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
docker run --rm datalust/seqcli:latest apikey create -t MockServer --token 12345678901234567890 --property=ProgramSource=MockServer -s http://$ip 
docker run --rm datalust/seqcli:latest apikey create -t MockClient --token 22345678901234567890 --property=ProgramSource=MockClient -s http://$ip 
docker run --rm datalust/seqcli:latest apikey create -t MockApi --token 32345678901234567890 --property=ProgramSource=MockApi -s http://$ip 
docker run --rm datalust/seqcli:latest apikey create -t MockLTP1 --token 42345678901234567890 --property=ProgramSource=MockLTP1 -s http://$ip 
docker run --rm datalust/seqcli:latest apikey create -t MockLTP2 --token 52345678901234567890 --property=ProgramSource=MockLTP2 -s http://$ip 