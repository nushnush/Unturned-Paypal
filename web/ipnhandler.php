<?php

$raw_post_data = file_get_contents('php://input');
$raw_post_array = explode('&', $raw_post_data);
$myPost = array();
foreach ($raw_post_array as $keyval)
{
    $keyval = explode('=', $keyval);
    if (count($keyval) == 2) $myPost[$keyval[0]] = urldecode($keyval[1]);
}

$req = 'cmd=_notify-validate';
if (function_exists('get_magic_quotes_gpc'))
{
    $get_magic_quotes_exists = true;
}
foreach ($myPost as $key => $value)
{
    if ($get_magic_quotes_exists == true && get_magic_quotes_gpc() == 1)
    {
        $value = urlencode(stripslashes($value));
    }
    else
    {
        $value = urlencode($value);
    }
    $req .= "&$key=$value";
}

$ch = curl_init('https://ipnpb.paypal.com/cgi-bin/webscr');
curl_setopt($ch, CURLOPT_HTTP_VERSION, CURL_HTTP_VERSION_1_1);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
curl_setopt($ch, CURLOPT_POSTFIELDS, $req);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 1);
curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 2);
curl_setopt($ch, CURLOPT_FORBID_REUSE, 1);
curl_setopt($ch, CURLOPT_HTTPHEADER, array(
    'Connection: Close'
));

if (!($res = curl_exec($ch)))
{
    // error_log("Got " . curl_error($ch) . " when processing IPN data");
    curl_close($ch);
    exit;
}
curl_close($ch);

if (strcmp ($res, "VERIFIED") == 0) {

	$item_name = $_POST['item_name'];
	$item_number = $_POST['item_number'];
	$payment_status = $_POST['payment_status'];
	$payment_amount = $_POST['mc_gross'];
	$payment_currency = $_POST['mc_currency'];
	$txn_id = $_POST['txn_id'];
	$receiver_email = $_POST['receiver_email'];
	$payer_email = $_POST['payer_email'];
	$steamid = $_POST['custom'];
	
	if(!is_int($steamid) || $steamid < 7000000000 || $payment_currency != "USD" || $payment_amount < 5 || strcmp($payment_status, "Completed") != 0) {
		return;
	}
	
	$servername = "localhost";
	$username = "username";
	$password = "password";
	$dbname = "myDB";
	$conn = new mysqli($servername, $username, $password, $dbname);
	
	if ($conn->connect_error) {
		die("Connection failed: " . $conn->connect_error);
	}
	
	$conn->query( sprintf("INSERT INTO `unturned_vips` (steamid, package, payer_email, time_created) VALUES ('%d', 'VIP+', '%s', CURRENT_TIMESTAMP())", $steamid, $payer_email) );
}

