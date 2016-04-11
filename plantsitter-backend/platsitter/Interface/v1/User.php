<?php 

do {
    $action = $_GET['action'];
    switch ($action) {

        case 'CheckUserExist':
            $email = $_GET['email'];
            $queryFind = $pdo->prepare('SELECT * FROM user WHERE email=:email');
            $queryFind->bindParam(':email', $email, PDO::PARAM_STR);
            $result = $queryFind->execute();
            if ($result) {
                $user = $queryFind->fetch();
                if ($user) {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['isExist'] = true;
                    break;
                } else {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['isExist'] = false;
                    break;
                }
            } else {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                $ApiResult['error_message'] = 'database error';
                break;
            }
            break;

        case 'GetSalt':
            $email = $_POST['email'];

            $queryFind = $pdo->prepare('SELECT salt from user WHERE email=:email');
            $queryFind->bindParam(':email', $email, PDO::PARAM_STR);
            $result = $queryFind->execute();
            if ($result) {
                $salt = $queryFind->fetch();
                if ($salt) {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = '';
                    $ApiResult['Salt'] = $salt['salt'];
                    break;
                } else {
                    $ApiResult['isSuccessed'] = true;
                    $ApiResult['error_code'] = 0;
                    $ApiResult['error_message'] = 'NO SALT';                    
                    $ApiResult['Salt'] = '';
                    break;
                }
            } else {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                $ApiResult['error_message'] = 'database error';
                break;
            }
            break;
        case 'Register':

            $email = $_POST['email'];
            $password = $_POST['password']; //the password is after md5 from client
            
            if ($email == '' || $password == '') {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_PARM_LACK;
                $ApiResult['error_message'] = 'Miss email or password ';
                break;
            }

            $queryFind = $pdo->prepare('SELECT * FROM user WHERE email=:email');
            $queryFind->bindParam(':email', $email, PDO::PARAM_STR);
            $resultFind = $queryFind->execute();
            if ($resultFind) {
                if ($queryFind->fetch()) {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_USER_ALEADY_EXIST;
                    $ApiResult['error_message'] = 'The email has already used';
                    break;
                }
            } else {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                $ApiResult['error_message'] = 'database error';
                break;
            }
            
            //get salt
            $salt = GetRandStr(10);
            $newPwd = md5($password.$salt);

            $queryInsert = $pdo->prepare('INSERT INTO user(email,password,salt) VALUES (:email,:password,:salt)');
            $queryInsert->bindParam(':email', $email, PDO::PARAM_STR);
            $queryInsert->bindParam(':password', $newPwd, PDO::PARAM_STR);
            $queryInsert->bindParam(':salt', $salt, PDO::PARAM_STR);

            $resultInsert = $queryInsert->execute();
            if ($resultInsert) {
                $uid = $pdo->lastInsertId();
                
                $accessToken = md5($sid);

                $queryInsert = $pdo->prepare('INSERT INTO accesstoken(uid,access_token) VALUES (:uid,:access_token)');
                $queryInsert->bindParam(':uid', $uid, PDO::PARAM_INT);
                $queryInsert->bindParam(':access_token', $accessToken, PDO::PARAM_STR);
                $resultInsert = $queryInsert->execute();
                if ($resultInsert) {
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

            } else {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                $ApiResult['error_message'] = 'database error';
                break;
            }

        case 'Login':

            $email = $_POST['email'];
            $password = $_POST['password']; // md5(md5(pwdInRaw)+salt)

            if ($email == '' || $password == '') {
                $ApiResult['isSuccessed'] = false;
                $ApiResult['error_code'] = API_ERROR_PARM_LACK;
                $ApiResult['error_message'] = 'Lack email or password or name';
                break;
            }

            $queryFind = $pdo->prepare('SELECT * FROM user WHERE email=:email && password=:password');
            $queryFind->bindParam(':email', $email, PDO::PARAM_STR);
            $queryFind->bindParam(':password', $password, PDO::PARAM_STR);
            $result = $queryFind->execute();
            if ($result) {
                $user = $queryFind->fetch();
                if ($user) {

                    $querySelect = $pdo->prepare('SELECT * FROM accesstoken WHERE uid=:uid');
                    $querySelect->bindParam(':uid', $user['uid'], PDO::PARAM_INT);
                    $resultSelect = $querySelect->execute();
                    if ($resultSelect) {
                        $accessToken = $querySelect->fetch();
                        if ($accessToken) {
                            $ApiResult['isSuccessed'] = true;
                            $ApiResult['error_code'] = 0;
                            $ApiResult['error_message'] = '';
                            $ApiResult['UserInfo'] = array('uid' => $user['uid'], 'access_token' => $accessToken['access_token']);
                            break;
                        }
                    } else {
                        $ApiResult['isSuccessed'] = false;
                        $ApiResult['error_code'] = API_ERROR_DATABASE_ERROR;
                        $ApiResult['error_message'] = 'database error';
                        break;
                    }
                } else {
                    $ApiResult['isSuccessed'] = false;
                    $ApiResult['error_code'] = API_ERROR_USER_NOT_EXIST;
                    $ApiResult['error_message'] = 'the user does not exists';
                    break;
                }
            } else {

            }
        default:
            #code...
            break;
    }

} while (0);


?>