<?php 

//require some file
require_once("./api_config.php");
require_once("./error_config.php");

do {

    if (in_array("$_GET[version]", $ApiVersions) && in_array("$_GET[interface]", $ApiInterfaces)) {
        if (isDebugMode) {
            $begin_time = microtime();
        }

        //the common APIResult to be returned
        $ApiResult = array('isSuccessed' => FALSE, 'error_code' => "0", "error_message" => "");

        //connect to database
        $db_source = "mysql:dbname=plantsitter;host=localhost";
        $pdo = new PDO($db_source, $apiConfig['DB_USERNAME'], $apiConfig['DB_PASSWORD'], array(
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES UTF8", ));
        
        do {
            //this action requires uid && accesstoken
            if ($_GET['uid'] && $_GET['access_token'] && !in_array("$_GET[interface]/$_GET[action]/$_GET[version]", $ApiUnauthorizedActions)) {
                
                //verfiy uid and accesstoken
                $query = $pdo->prepare("SELECT access_token FROM access_token WHERE access_token=:access_token && uid=:uid");
                $query->bindParam(':access_token', $_GET['access_token'], PDO::PARAM_STR);
                $query->bindParam(':uid', $_GET['uid'], PDO::PARAM_INT);

                if (!$query->execute()) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_DATABASE_ERROR;
                    $ApiResult['error_message'] = $query->errorInfo();
                    break;
                }

                $accessToken = $query->fetch();
                if (!$accessToken) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult["error_code"] = API_ACCESS_TOKEN_INVAID;
                    $ApiResult["error_message"] = "Access Token invaid";
                    break;
                }

                //uid and accesstoken is matched
                require_once("./Interface/$_GET[version]/$_GET[interface].php");
            }
            //register and login interface
            else {
                if (!in_array("$_GET[interface]/$_GET[action]/$_GET[version]", $ApiUnauthorizedActions)) {
                    $ApiResult['error_code'] = API_ERROR_PARM_LACK;
                    $ApiResult['error_message'] = 'LACK UID AND ACCESSTOKEN';
                    break;
                } else {
                    require_once("./Interface/$_GET[version]/$_GET[interface].php");
                }
            }

        } while (0);

        if (isDebugMode) {
            $ApiResult['executionTime'] = microtime() - $begin_time;
        } else unset($ApiResult['error_message']);

        header('Content-Type: application/json');

        echo json_encode($ApiResult);
    } else {
        http_response_code(400);
        break;
    }


} while (0);


?>