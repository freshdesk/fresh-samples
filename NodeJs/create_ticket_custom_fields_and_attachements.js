// Requires unirest module. To install:
// npm install unirest

var unirest = require('unirest');
var fs = require('fs');

var API_KEY = "YOUR_API_KEY";
var FD_ENDPOINT = "YOUR_DOMAIN";

var PATH = "/api/v2/tickets";
var enocoding_method = "base64";
var auth = "Basic " + new Buffer(API_KEY + ":" + 'X').toString(enocoding_method);
var URL =  "https://" + FD_ENDPOINT + ".freshdesk.com"+ PATH;
var tags = ['Testing1', 'Testing2'];
var custom_fields = {'department': 'IT'};
var fields = {
  'email': 'email@yourdomain.com',
  'subject': 'Ticket subject',
  'description': 'Ticket description.',
  'status': 2,
  'priority': 1,
  'tags': tags,
  'custom_fields':custom_fields
}

var headers = {
  'Authorization': auth
}

unirest.post(URL)
  .headers(headers)
  .field(fields)
  .attach('attachments[]', fs.createReadStream('/path/to/file1.ext'))
  .attach('attachments[]', fs.createReadStream('/path/to/file2.ext'))
  .end(function(response){
    console.log(response.body)
    console.log("Response Status : " + response.status)
    if(response.status == 201){
      console.log("Location Header : "+ response.headers['location'])
    }
    else{
      console.log("X-Request-Id :" + response.headers['x-request-id']);
    }
  });
