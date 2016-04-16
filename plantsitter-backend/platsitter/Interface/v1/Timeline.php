<?php 

do {
    $action = $_GET['action'];
    switch ($action) {
        case 'UploadData':
            {
                $pid = $_POST['pid'];
                $uid = $_POST['uid'];
                $gid = $_POST['gid'];
                $soil_moisture = $_POST['soil_moisture'];
                $envi_temp = $_POST['envi_temp'];
                $envi_moisture = $_POST['envi_moisture'];
                $light = $_POST['light'];
                $time = $_POST['time']; // format should be 2016/07/04 12:00

                if ($pid == '' || $uid == '' || gid == '' || $soil_moisture == '' || $envi_temp == '' || $envi_moisture == '' || $light == '' || $time == '') {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = 'Lack necessary params of the uploading data.';
                    break;
                }

                $queryInsert = $pdo->prepare('INSERT INTO user_plant_timeline(pid,uid,gid,soil_moisture,envi_temp,envi_moisture,light,time) 
                                            VALUES (:pid,:uid,:gid,:soil_moisture,:envi_temp,:envi_moisture,:light,:time)');

                $queryInsert->bindParam(':pid', $pid, PDO::PARAM_INT);
                $queryInsert->bindParam(':uid', $uid, PDO::PARAM_INT);
                $queryInsert->bindParam(':gid', $gid, PDO::PARAM_INT);
                $queryInsert->bindParam(':soil_moisture', $soil_moisture, PDO::PARAM_STR);
                $queryInsert->bindParam(':envi_temp', $envi_temp, PDO::PARAM_STR);
                $queryInsert->bindParam(':envi_moisture', $envi_moisture, PDO::PARAM_STR);
                $queryInsert->bindParam(':light', $light, PDO::PARAM_STR);
                $queryInsert->bindParam(':time', $time, PDO::PARAM_STR);

                $result = $queryInsert->execute();
                if ($result) {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
            }
            break;
        case "GetTimelineData":
            {
                $filterKinds = array('byNumber', 'byMonths', 'byYears', 'byDays', 'betweenDates');

                $endTime = date('Y-m-d h:i:s');

                $gid = $_GET['gid'];
                $filter_kind = $_GET['filter_kind'];
                $filter_value = $_GET['filter_value']; //EXAMPLES: byNumber:10, byMonths:2,byYears:3,byDays:10,betweenDate: 2016-01-12~2016-03-20

                if ($filter_kind != '' && $filter_value == '') {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'Must define filter_value'.$filter_kind;
                    break;
                }

                if ($filter_kind != '' && !in_array($filter_kind, $filterKinds)) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'Invalid filter kind of '.$filter_kind;
                    break;
                }

                $findQuery = $pdo->prepare('SELECT * FROM user_plan WHERE gid=:gid');
                $findQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                if (!$findQuery->execute()) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
                $findResult = $findQuery->fetch();
                if (!$findResult) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = "No such plan";
                    break;
                }
                if ($filter_kind == '' && $filter_value == '') {
                    $filter_kind = 'byNumber';
                    $filter_value = 2;
                }
                $getQuery;
                if ($filter_kind == 'byNumber') {
                    $getQuery = $pdo->prepare('SELECT * FROM user_plant_timeline WHERE gid=:gid LIMIT :number');
                    $getQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                    $getQuery->bindParam(':number', intval($filter_value), PDO::PARAM_INT);
                } else if ($filter_kind == 'byMonths') {

                    $getQuery = $pdo->prepare('SELECT * FROM user_plant_timeline WHERE gid=:gid AND time between :startTime and :endTime');
                    $getQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                    $getQuery->bindParam(':endTime', $endTime, PDO::PARAM_STR);
                    $getQuery->bindParam(':startTime', date('Y-m-d h:i:s', strtotime('-'.$filter_value.' months')), PDO::PARAM_STR);
                } else if ($filter_kind == 'byYears') {

                    $getQuery = $pdo->prepare('SELECT * FROM user_plant_timeline WHERE gid=:gid AND time between :startTime and :endTime');
                    $getQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                    $getQuery->bindParam(':endTime', $endTime, PDO::PARAM_STR);
                    $getQuery->bindParam(':startTime', date('Y-m-d h:i:s', strtotime('-'.$filter_value.' years')), PDO::PARAM_STR);
                } else if ($filter_kind == 'byDays') {

                    $getQuery = $pdo->prepare('SELECT * FROM user_plant_timeline WHERE gid=:gid AND time between :startTime and :endTime');
                    $getQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                    $getQuery->bindParam(':endTime', $endTime, PDO::PARAM_STR);
                    $getQuery->bindParam(':startTime', date('Y-m-d h:i:s', strtotime('-'.$filter_value.' days')), PDO::PARAM_STR);
                } else if ($filter_kind == 'betweenDates') {

                    $dateRange = $filter_value;
                    $dates = explode('~', $dateRange);
                    $startDate = $dates[0];
                    $endingDate = $dates[1];

                    if ($startDate == '' || $endingDate == '') {
                        $ApiResult['isSuccessed'] = false;
                        $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                        $ApiResult['error_message'] = 'Between dates value is invalid.'.' '.$startDate.$endingDate;
                        break;
                    }

                    $getQuery = $pdo->prepare('SELECT * FROM user_plant_timeline WHERE gid=:gid AND time between :startTime and :endTime');
                    $getQuery->bindParam(':gid', $gid, PDO::PARAM_INT);
                    $getQuery->bindParam(':endTime', $endingDate, PDO::PARAM_STR);
                    $getQuery->bindParam(':startTime', $startDate, PDO::PARAM_STR);
                }

                if (!$getQuery->execute()) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $pdo->errorInfo();
                    break;
                }
                $resultGet = $getQuery->fetchAll();
                $ApiResult['isSuccessed'] = true;
                $ApiResult['error_code'] = 0;
                $ApiResult['error_message'] = '';
                $ApiResult['Plans'] = $resultGet;
                break;
            }
            break;
        default:
            #code...
            break;
    }

} while (0);


?>