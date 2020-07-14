# The AllowList repo

The tech used:
- c# console app
- xUnit and MOQ
- GitHub Actions Workflow manifest

Use-cases:
- Generate an nginx .conf file of all the the GitHub Actions Runners IPv4 ranges 
 
## Background

Your GitHub Action Workflow manifest may need to reach out to your own managed infrastructure (e.g. non-Azure DC).  

To secure your infrastructure (IP spoofing to one side for a moment), you will need to know which IPs to allow access from.

The specific use-case that this repo is dealing with is one that generates an `allow list` - using this term instead of whitelist to support BlackLivesMatter - to have as an `includes` file in an [nginx](https://www.nginx.com/resources/glossary/nginx/) configuration (conf) file as in:

```conf
server {
    ...    
    location / {
        ...
        include conf.d/includes/github-actions-runners.conf;   
        ...
    }
}
``` 

Microsoft release an updated list of their IPv4 ranges, weekly.  Included in this are the GitHub Actions Runners.  This is the download page - https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519.  There is a link in this page, however, the URI of this JSON file changes weekly.

## What does this repo do?

This repo does the following:
 - works out what the JSON endpoint is for the current date, 
 - then downloads it, 
 - parses it, 
 - extracts only those applicable IPv4 ranges, 
 - and generates an nginx `.conf` file,
 - pushes the changed file to the `/includes` folder,
 - raises a PR.
 
The GitHub Actions Manifest that is included here:
 - allows you to run this whenever using [manual trigger](https://github.blog/changelog/2020-07-06-github-actions-manual-triggers-with-workflow_dispatch/) using and 
 - every Tuesday (day after the scheduled update).  

# Getting Started

TBC