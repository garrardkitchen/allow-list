# Reason for repo

Your GitHub Action may need to reach out to your own managed infrastructure (e.g. non-Azure DC).  

To secure your infrastructure (IP spoofing to one side for a moment), you will need to know which IPs to allow access from.

The use-case that this repo is dealing with is to generate an `allow list` - using this term instead of whitelist to support BlackLivesMatter - to have as an `includes` file in an [nginx](https://www.nginx.com/resources/glossary/nginx/) configuration (conf) file as in:

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

Microsoft release an updated list of IPv4 ranges where their services.  Included in this are the GitHub Actions Runners.  This is the download page - https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519.  There is a link in this page that changes weekly but will be active the following week.

What this repo does is:
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