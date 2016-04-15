<?php 

do {
    $action = $_GET['action'];
    switch ($action) {
        case 'UploadData':
            {
                $pid = $_POST['pid'];
                $uid=$_POST['uid'];
                $gid=$_POST['gid'];
                $soil_moisture=$_POST['soil_moisture'];
                $envi_temp=$_POST['envi_temp'];
                $envi_moisture=$_POST['envi_moisture'];
                $light=$_POST['light'];
                $time=$_POST['time'];
                
                $queryInsert = $pdo->prepare('INSERT INTO user_plant_timeline(pid,uid,gid,soil_moisture,envi_temp,envi_moisture,light,time) 
                                            VALUES(:pid,:uid,:gid,:soil_moisture,:envi_temp,:envi_moisture,:light,:time)');
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
        default:
            #code...
            break;
    }

} while (0);


?>