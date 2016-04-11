<?php 

//包含文件
require_once("./api_config.php");
require_once("./error_config.php");
require_once("./HttpClient.class.php");
do {

    if (in_array("$_GET[version]", $ApiVersions) && in_array("$_GET[interface]", $ApiInterfaces)) {
        if (isDebugMode) {
            $begin_time = microtime();
        }

        //用于返回的JSON data
        $ApiResult = array('isSuccessed' => FALSE, 'error_code' => "0", "error_message" => "");

        //连接到数据库
        $db_source = "mysql:dbname=plantsitter;host=localhost";
        $pdo = new PDO($db_source, $apiConfig['DB_USERNAME'], $apiConfig['DB_PASSWORD'], array(
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
        PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES UTF8", ));

        do {
            //对于要传入AccessToken和eid的操作
            if ($_GET['sid'] && $_GET['access_token'] && !in_array("$_GET[interface]/$_GET[action]/$_GET[version]", $ApiUnauthorizedActions)) {
                //验证AccessToken 和 UID
                $query = $pdo->prepare("SELECT access_token FROM accesstoken WHERE access_token=:access_token && uid=:uid");
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

                //验证完，调用接口内容
                require_once("./Interface/$_GET[version]/$_GET[interface].php");
            }
            //登录和注册接口
            else {
                if (!in_array("$_GET[interface]/$_GET[action]/$_GET[version]", $ApiUnauthorizedActions)) {
                    $ApiResult['error_code'] = API_ERROR_PARM_LACK;
                    $ApiResult['error_message'] = 'LACK UID AND ACCESSTOKEN FROM GET REQUEST';
                    break;
                } else {
                    require_once("./Interface/$_GET[version]/$_GET[interface].php");
                }
            }

        } while (0);

        if (API_DEBUG) {
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