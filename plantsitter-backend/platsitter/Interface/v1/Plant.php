<?php 

do {
    $action = $_GET['action'];
    switch ($action) {
        case 'GetPlantInfo':
            {
                $pid = $_POST['pid'];

                $queryFind = $pdo->prepare('SELECT * FROM plant WHERE pid=:pid');
                $queryFind->bindParam(':pid', $pid, PDO::PARAM_INT);
                $result = $queryFind->execute();
                if ($result) {
                    $plant = $queryFind->fetch();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plant'] = $plant;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'AddPlant':
            {
                $name_c = $_POST['name_c'];
                $name_e = $_POST['name_e'];
                $soil_moisture = $_POST['soil_moisture'];
                $envi_moisture = $_POST['envi_moisture'];
                $envi_temp = $_POST['envi_temp'];
                $light = $_POST['light'];

                $querySearch;
                if ($name_e) {
                    $querySearch = $pdo->prepare('SELECT * FROM plant WHERE name_e=:name_e');
                    $querySearch->bindParam(':name_e', $name_e, PDO::PARAM_STR);
                } else if ($name_c) {
                    $querySearch = $pdo->prepare('SELECT * FROM plant WHERE name_c=:name_c');
                    $querySearch->bindParam(':name_c', $name_c, PDO::PARAM_STR);
                }
                $searchResult = $querySearch->execute();
                if ($searchResult) {
                    if($querySearch->fetch()){
                        $ApiResult['isSuccessed'] = false;
                        $ApiResult['error_code'] = 0;
                        $ApiResult['error_message'] = 'The plant with this name has existed.';
                        break;
                    }   
                }

                $queryAdd = $pdo->prepare('INSERT INTO plant(name_c,name_e,soil_moisture,envi_moisture,envi_temp,light) VALUES(:name_c,:name_e,:soil_moisture,:envi_moisture,:envi_temp,:light)');
                $queryAdd->bindParam(':name_c', $name_c, PDO::PARAM_STR);
                $queryAdd->bindParam(':name_e', $name_e, PDO::PARAM_STR);
                $queryAdd->bindParam(':soil_moisture', $soil_moisture, PDO::PARAM_STR);
                $queryAdd->bindParam(':envi_moisture', $envi_moisture, PDO::PARAM_STR);
                $queryAdd->bindParam(':envi_temp', $envi_temp, PDO::PARAM_STR);
                $queryAdd->bindParam(':light', $light, PDO::PARAM_STR);

                $result = $queryAdd->execute();
                if ($result) {
                    $plantID = $pdo->lastInsertId();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['PlantID'] = $plantID;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = $queryAdd->errorInfo();
                    break;
                }
            }
            break;
        case 'SearchPlant':
            {
                //Search by pid or name_c or name_e, those params are in post date
                //the priority is pid->name_e->name_c
                //which means that if post data contains pid, name_c or name_e will be ignored.
                $pid = $_POST['pid'];
                $name_c = $_POST['name_c'];
                $name_e = $_POST['name_e'];

                $querySearch;
                if ($pid) {
                    $querySearch = $pdo->prepare('SELECT * FROM plant WHERE pid=:pid');
                    $querySearch->bindParam(':pid', $pid, PDO::PARAM_INT);
                } else if ($name_e) {
                    $querySearch = $pdo->prepare('SELECT * FROM plant WHERE name_e LIKE %:name_e%');
                    $querySearch->bindParam(':name_e', $name_e, PDO::PARAM_STR);
                } else if ($name_c) {
                    $querySearch = $pdo->prepare('SELECT * FROM plant WHERE name_c LIKE %:name_c%');
                    $querySearch->bindParam(':name_c', $name_c, PDO::PARAM_STR);
                }

                $result = $querySearch->execute();
                if ($result) {
                    $plant = $querySearch->fetch();
                    if ($plant) {
                        $ApiResult['isSuccessed'] = true;
                        $ApiResult['error_code'] = 0;
                        $ApiResult['error_message'] = '';
                        $ApiResult['Plant'] = $plant;
                    } else {
                        $ApiResult['isSuccessed'] = false;
                        $ApiResult['error_code'] = 0;
                        $ApiResult['error_message'] = 'No plant found';
                    }

                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
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