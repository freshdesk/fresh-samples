<?php 

$api_key = "API_KEY";
$password = "x";
$yourdomain = "YOUR_DOMAIN";

$url = "https://$yourdomain.freshdesk.com/api/v2/tickets";

$ch = curl_init($url);

curl_setopt($ch, CURLOPT_HEADER, true);
curl_setopt($ch, CURLOPT_USERPWD, "$api_key:$password");
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

$server_output = curl_exec($ch);
$info = curl_getinfo($ch);
$header_size = curl_getinfo($ch, CURLINFO_HEADER_SIZE);
$headers = substr($server_output, 0, $header_size);
$response = substr($server_output, $header_size);

if($info['http_code'] == 200) {
  echo "Tickets fetched successfully, the response is given below \n";
  echo "Response Headers are \n";
  echo $headers."\n";
  echo "Response Body \n";
  echo "$response \n";
} else {

  if($info['http_code'] == 404)
  {
    echo "Error, Please check the end point \n";
  } else {
    echo "Error, HTTP Status Code : " . $info['http_code'] . "\n";
    echo "Headers are ".$headers;
    $response_data = json_decode($response);

    foreach ($response_data->{'errors'} as $error) {
        echo "Field : ".$error->{'field'} . " | Message : ".$error->{'message'} . " | Code : ".$error->{'code'} ."\n";
    }  
  }
}

curl_close($ch);

?>