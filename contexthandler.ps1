function Add-Migration-Catering {
    $name = Read-Host "Enter migration name:"
    echo $name
    dotnet ef migrations add $name --project .\Catering.Models\ --startup-project .\CateringDataProcessingPlatform\;
}

function Remove-Migration-Catering {
    dotnet ef migrations remove --project .\Catering.Models\ --startup-project .\CateringDataProcessingPlatform\;
}

function Update-Database-Catering {
    dotnet ef database update --project .\Catering.Models\ --startup-project .\CateringDataProcessingPlatform\;
}

function Add-Migration-User {
    $name = Read-Host "Enter migration name:"
    echo $name
    dotnet ef migrations add $name  --project .\UserPlatform.Shared\ --startup-project .\UserPlatform\;
}

function Remove-Migration-User {
    dotnet ef migrations remove --project .\UserPlatform.Shared\ --startup-project .\UserPlatform\;
}

function Update-Database-User {
    dotnet ef database update --project .\UserPlatform.Shared\ --startup-project .\UserPlatform\;
}


$isDotnetEfPresent = (Get-ItemPropertyValue -LiteralPath 'HKLM:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' -Name Release) -ge 378389

echo "Dotnet Ef helper script..."

if($false -eq $isDotnetEfPresent){
        echo "Dotnet EF 4.5 or newer is not installed on the machine."      
}
else {
    while(1 -eq 1){
    $option = Read-Host "Do you want to (C)reate migration, (U)pdate database, (R)emove the newest non-applied migration or (E)xit the script?"
        if($option -eq 'C'){
            $option = Read-Host "(U)ser or (C)atering?"
            if($option -eq 'U'){
                Add-Migration-User
            }
            elseif($option -eq 'C'){
                Add-Migration-Catering
            }
        }
        elseif($option -eq 'U'){
            $option = Read-Host "(U)ser or (C)atering?"
            if($option -eq 'U'){
                Update-database-User
            }
            elseif($option -eq 'C'){
                Update-database-Catering
            }
        }
        elseif($option -eq 'R'){
            $option = Read-Host "(U)ser or (C)atering?"
            if($option -eq 'U'){
                Remove-Migration-User
            }
            elseif($option -eq 'C'){
                Remove-Migration-Catering
            }
        }
        elseif($option -eq 'E'){
            Exit
        }
        else{
            echo "Unknown command."
        }

    }
}
