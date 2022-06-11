import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/home_controller.dart';
import 'package:term7moviemobile/routes/routes.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'firebase_options.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  Get.put(AuthController());
  Get.put(HomeController());
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'F-Cine',
      theme: MyTheme.myLightTheme,
      debugShowCheckedModeBanner: false,
      initialRoute: Routes.getSplashRoute(),
      getPages: Routes.routes,
    );
  }
}
