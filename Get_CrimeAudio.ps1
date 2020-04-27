Get-ChildItem "D:\Dev\FiveM\FivePDAudio\POLICE_SCANNER_original\CRIMES" | ForEach-Object {
    $keyword = $_.Name -replace "CRIME_","" -replace "_0"," 0" -replace ".wav","" -replace "_"," "
    Write-Host "crimeAudio.Add(new KeyValuePair<string,string>(`"$($keyword.ToLower())`", @`"CRIMES/$($_.Name -replace ".wav",".ogg")`"));"


}