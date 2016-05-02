# PlantSitter

基于 Windows 10 的植物成长监控应用。

##目录说明

###Design 

存放一些设计相关的文件

### Plantsitter Backend

服务端的代码


### PlantSitter 

客户端的代码，包括运行在 Windows 10 Desktop/Mobile 设备上的和运行在安装了 Windows 10 IoT Core 的树莓派上的。

#### PlantSitter/PlantSitter

Windows 10 Desktop/Mobile 客户端主项目

#### PlantSitter/PlantSitterCustomControl

一些自定义控件的代码

#### PlantSitter/PlantSitter-Resp

树莓派端的代码

#### PlantSitter/PlantSitterShared

用于Windows 10 Desktop/Mobile 客户端和树莓派端共享的代码，包括 Model 和 API 请求等

#### PlantSitter/Sensor.Env

环境湿度温度传感器的读取

#### PlantSitter/Sensor.Light

光线强度传感器的读取

#### PlantSitter/Sensor.Soil

土壤湿度传感器的读取

#### PlantSitter/UnitTestProject

单元测试项目