$p = 'Test123!'
$ph = $(echo $p | docker run --rm -i datalust/seq config hash)

$ph

$name = "seqH6";

$isRunning = 0;
docker run --name $name -d --restart unless-stopped -e ACCEPT_EULA=Y -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="$ph" -p 81:80  -p 5342:5341 datalust/seq:2024

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
docker run --rm datalust/seqcli:latest apikey create -t cateringLTPDev --token 12345678901234567890 --property=ProgramSource=DevCateringLTP -s http://$ip -connect-username="Admin" --connect-password="Test123!"
docker run --rm datalust/seqcli:latest apikey create -t userAPIDev --token 22345678901234567890 --property=ProgramSource=DevUserAPI -s http://$ip -connect-username="Admin" --connect-password="Test123!"
docker run --rm datalust/seqcli:latest apikey create -t cateringLTPLive --token 32345678901234567890 --property=ProgramSource=LiveCateringLTP -s http://$ip -connect-username="Admin" --connect-password="Test123!"
docker run --rm datalust/seqcli:latest apikey create -t userAPILive --token 42345678901234567890 --property=ProgramSource=LiveUserAPI -s http://$ip -connect-username="Admin" --connect-password="Test123!"
docker run --rm datalust/seqcli:latest apikey create -t cateringAPIDev --token 52345678901234567890 --property=ProgramSource=DevCateringAPI -s http://$ip -connect-username="Admin" --connect-password="Test123!"
docker run --rm datalust/seqcli:latest apikey create -t cateringAPILive --token 62345678901234567890 --property=ProgramSource=LiveCateringAPI -s http://$ip -connect-username="Admin" --connect-password="Test123!"