# Set environment variables
setx ASPNETCORE_ENVIRONMENT "Development" /M
setx ENCRYPTION_KEY "YW8sdg9F7QQmahYupfTb3S0ZK8Owgqd9CylaW9YPbF0=" /M
setx ENCRYPTION_IV "MGnCE1od5V53T80Lwib7kQ==" /M

# Output the current environment variable value


 # Instructions for verification
Write-Host "You can verify them by running:"
Write-Host "echo `\$env:ASPNETCORE_ENVIRONMENT"
Write-Host "echo `\$env:ENCRYPTION_KEY"
Write-Host "echo `\$env:ENCRYPTION_IV"