# Logs

One activity that is not a part of the Net.Incident4 application is the loading of network incident logs.  This folder is a loose collection of utilities to perform loading of logs.

I am developing on my home laptop, so I do not have all of the available resources that I would normally have in a business system development.

I started with a Netgear 300 wireless router.  I upgraded to Netgear R6220 wireless router.  Each Netgear router logs are available by e-mail, or logging on as the administrator copying the logs to a text file.  I did the latter.

Finally, I have my IIS Logs load.

## Manual Logs Example:

### Get logs from device

I logged into my router and copied the logs and pasted into a text file called 'NGR6220-2022-04-27.txt'.

### Filter logs extracting network incidents

I ran it against a simple fliter script.

    C:\> LogFilter NGR6220-2022-04-27

It created 2 files as follows:

- NGR6220-2022-04-27-admin.txt
- NGR6220-2022-04-27-probe.txt

The 'admin' file is informational.  I check it to make sure, I know who is logging into the route.  The following is a section of the 'probe' file:

    [DoS attack: RST Scan] from source: 69.147.65.252:443, Wednesday, April 27,2022 17:16:30       
    [DoS attack: TCP Port Scan] from source: 89.248.165.65:46430, Wednesday, April 27,2022 08:33:55       
    [DoS attack: NULL Scan] from source: 39.107.97.32:45747, Wednesday, April 27,2022 07:00:47       
    [DoS attack: RST Scan] from source: 142.250.190.68:443, Wednesday, April 27,2022 06:26:54       
    [DoS attack: RST Scan] from source: 23.202.218.245:443, Wednesday, April 27,2022 06:12:21       
    [DoS attack: RST Scan] from source: 23.201.56.107:443, Wednesday, April 27,2022 05:32:55       
    [DoS attack: RST Scan] from source: 194.88.104.81:80, Wednesday, April 27,2022 02:45:05       
    [DoS attack: RST Scan] from source: 142.250.190.68:443, Wednesday, April 27,2022 01:52:03       
    [DoS attack: WinNuke Attack] from source: 122.224.125.74:13967, Wednesday, April 27,2022 00:48:23       
    [DoS attack: RST Scan] from source: 17.253.25.207:80, Wednesday, April 27,2022 00:43:39       

### Loading database with server and incident type set

The following is a table of incident types.  You will need to know the appropriate IncidentTypeId.

| IncidentTypeId | Short Description | Description |
| --- | ----------- | ----------- |
| 1 | DoS | Denial-of-service attack |
| 2 | DIR | Directory traversal |
| 3 | VS | ViewState |
| 4 | XSS | Cross Site Scripting |
| 5 | PHP | PHP |
| 6 | SQL | SQL Injection |
| 7 | Multiple | Multiple Types |
| 8 | Unk | Unknown |

The NetGear R6220's server id is 4.  To load the DoS incidents launch PowerShell and execute the following command:

    PS C:\> .\LogsLoad-NG2.ps1 -server ".\Express" -dbName "NetIncidentIdentity04" -filePath ".\NGR6220-2022-04-27-probe.txt" -serverId 4 -incidentTypeId 1

## Another Load Logs Example:


    python logs_load.py --logType NGR --server .\Express --serverId 4 --filePath ..\NGR6220-2022-04-27-probe.txt
