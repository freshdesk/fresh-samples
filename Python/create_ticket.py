## This script requires "requests": http://docs.python-requests.org/
## To install: pip install requests

import requests
import json

api_key = "YOUR_API_KEY"
domain = "YOUR_DOMAIN"
password = "x"

headers = { 'Content-Type' : 'application/json' }

ticket = {
    'subject' : 'Ticket title',
    'description' : 'Ticket detail',
    'email' : 'example@example.com',
    'priority' : 1,
    'status' : 2,
    'cc_emails' : ['sample_email@domain.com', 'user_email@domain.com']
}

r = requests.post("https://"+ domain +".freshdesk.com/api/v2/tickets", auth = (api_key, password), headers = headers, data = json.dumps(ticket))

if r.status_code == 201:
  print (f"Ticket created successfully, the response is given below {r.content}")
  print (f"Location Header :  {r.headers['Location']"}
else:
  print (Failed to create ticket, errors are displayed below,)
  response = json.loads(r.content)
  print (response["errors"])

  print (f"x-request-id : {r.headers['x-request-id']"}
  print (f"Status Code : {str(r.status_code)"}
