[cmdletbinding()]
param(
    [string]$server="ticketsrusdb.postgres.database.azure.com",
    [int]$port=5432,
    [string]$database="postgres",
    [parameter(Mandatory=$true)]
    [string]$user,
    [string]$password,
    [string]$localFolder=".",
    [string]$backupName=((get-date).ToString("yyyyMMdd.hh.mm.ss")+".sql"),
    [string]$schema="ticketsdb"
)

if($password -eq $null) {
    $ss = read-host "Enter your password" -AsSecureString
    $password = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($password))
}
docker run --rm -e PGPASSWORD=$password -v "$($localFolder):/usr/backupoutput" -it postgres pg_dump -h $server -p 5432 -U $user -f /usr/backupoutput/$backupName -d $database --schema=$schema