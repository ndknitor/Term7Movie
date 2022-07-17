import 'package:awesome_notifications/awesome_notifications.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/booking_controller.dart';
import 'package:term7moviemobile/controllers/main_controller.dart';
import 'package:term7moviemobile/controllers/onboarding_controller.dart';
import 'package:term7moviemobile/controllers/showtime_controller.dart';
import 'package:term7moviemobile/routes/routes.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'firebase_options.dart';

void main() async {
  AwesomeNotifications().initialize(
      'resource://drawable/res_logo_icon',
      [
        NotificationChannel(
            channelGroupKey: 'basic_channel_group',
            channelKey: 'basic_channel',
            channelName: 'Basic notifications',
            channelDescription: 'Notification channel for basic tests',
            defaultColor: MyTheme.primaryColor,
            importance: NotificationImportance.High,
            channelShowBadge: true)
      ],
      // Channel groups are only visual and are not required
      channelGroups: [
        NotificationChannelGroup(
            channelGroupkey: 'basic_channel_group',
            channelGroupName: 'Basic group')
      ],
      debug: true);
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  Get.put(AuthController());
  Get.put(MainController());
  Get.put(OnBoardingController());
  Get.put(BookingController());
  Get.put(ShowtimeController());
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
