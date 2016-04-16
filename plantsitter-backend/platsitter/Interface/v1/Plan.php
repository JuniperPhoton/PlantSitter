<?php 

do {
    $action = $_GET['action'];
    switch ($action) {
        case 'GetAllPlans':
            {
                $uid = $_GET['uid'];
                $queryFind = $pdo->prepare('SELECT * FROM user_plan WHERE uid=:uid');
                $queryFind->bindParam(':uid', $uid, PDO::PARAM_INT);
                $result = $queryFind->execute();
                if ($result) {
                    $plans = $queryFind->fetchAll();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plans'] = $plans;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'GetPlan':
            {
                $gid = $_GET['gid'];
                $queryFind = $pdo->prepare('SELECT * FROM user_plan WHERE gid=:gid');
                $queryFind->bindParam(':gid', $gid, PDO::PARAM_INT);
                $result = $queryFind->execute();
                if ($result) {
                    $plan = $queryFind->fetch();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plan'] = $plan;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'DeletePlan':
            {
                $gid = $_GET['gid'];
                $queryFind = $pdo->prepare('DELETE FROM user_plan WHERE gid=:gid');
                $queryFind->bindParam(':gid', $gid, PDO::PARAM_INT);
                
                $result = $queryFind->execute();
                if ($result) {
                    $plan = $queryFind->fetch();

                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    break;
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                    $ApiResult['error_message'] = 'database error';
                    break;
                }
            }
            break;
        case 'AddPlan':
            {
                $pid = $_POST['pid'];
                $uid = $_POST['uid'];
                $name = $_POST['name'];
                $time=$_POST['time'];
                
                $queryAdd = $pdo->prepare('INSERT INTO user_plan(uid,pid,name,time) VALUES(uid=:uid,pid=:pid,name=:name,time=:time)');
                $queryAdd->bindParam(':uid', $uid, PDO::PARAM_INT);
                $queryAdd->bindParam(':name', $name, PDO::PARAM_STR);
                $queryAdd->bindParam(':time', $time, PDO::PARAM_STR);

                $result = $queryAdd->execute();
                if ($result) {
                    $newid = $pdo->lastInsertId();
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Plan'] = array('gid' => $newid, 'name' => $name, 'pid' => $pid, 'uid' => $uid,'time'=>$time);
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