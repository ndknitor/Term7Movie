import 'package:get/get.dart';
import 'package:term7moviemobile/screens/home_screen.dart';
import 'package:term7moviemobile/screens/login_screen.dart';
import 'package:term7moviemobile/screens/onboarding_screen.dart';
import 'package:term7moviemobile/screens/splash_screen.dart';

class Routes {
  static String home = "/";
  static String splash = "/splash";
  static String onboard = "/onboard";
  static String login = "/login";

  static String getSplashRoute() => splash;
  static String getOnBoardRoute() => onboard;
  static String getHomeRoute() => home;
  static String getLoginRoute() => login;

  static List<GetPage> routes = [
    GetPage(
      name: home,
      page: () => HomeScreen(),
      transition: Transition.fade,
      transitionDuration: Duration(milliseconds: 500),
    ),
    GetPage(name: splash, page: () => SplashScreen()),
    GetPage(name: onboard, page: () => OnBoardingScreen()),
    GetPage(name: login, page: () => LoginScreen()),
  ];
}
