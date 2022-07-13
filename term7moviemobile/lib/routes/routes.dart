import 'package:get/get.dart';
import 'package:term7moviemobile/screens/booking_history_screen.dart';
import 'package:term7moviemobile/screens/booking_screen.dart';
import 'package:term7moviemobile/screens/checkout_screen.dart';
import 'package:term7moviemobile/screens/details_screen.dart';
import 'package:term7moviemobile/screens/history_detail.dart';
import 'package:term7moviemobile/screens/location_screen.dart';
import 'package:term7moviemobile/screens/login_screen.dart';
import 'package:term7moviemobile/screens/main_screen.dart';
import 'package:term7moviemobile/screens/movie_list_screen.dart';
import 'package:term7moviemobile/screens/onboarding_screen.dart';
import 'package:term7moviemobile/screens/profile_screen.dart';
import 'package:term7moviemobile/screens/splash_screen.dart';

class Routes {
  static String home = "/";
  static String splash = "/splash";
  static String onboard = "/onboard";
  static String login = "/login";
  static String movies = "/movies";
  static String profile = "/profile";
  static String location = "/location";
  static String detail = "/detail/:id";
  static String booking = "/booking/:id";
  static String checkout = "/checkout";
  static String history = "/history";
  static String historyDetail = "/history/:id";

  static String getSplashRoute() => splash;
  static String getOnBoardRoute() => onboard;
  static String getHomeRoute() => home;
  static String getLoginRoute() => login;
  static String getMoviesRoute() => movies;
  static String getProfileRoute() => profile;
  static String getLocationRoute() => location;
  static String getDetailRoute() => detail;
  static String getBookingRoute() => booking;
  static String getCheckoutRoute() => checkout;
  static String getHistoryRoute() => history;
  static String getHistoryDetailRoute() => historyDetail;

  static List<GetPage> routes = [
    GetPage(
      name: home,
      page: () => MainScreen(),
    ),
    GetPage(name: splash, page: () => SplashScreen()),
    GetPage(name: onboard, page: () => OnBoardingScreen()),
    GetPage(name: login, page: () => LoginScreen()),
    GetPage(name: movies, page: () => MovieListScreen()),
    GetPage(name: profile, page: () => ProfileScreen()),
    GetPage(name: location, page: () => LocationScreen()),
    GetPage(name: detail, page: () => MovieDetailScreen()),
    GetPage(name: booking, page: () => BookingScreen()),
    GetPage(name: checkout, page: () => CheckOutScreen()),
    GetPage(name: history, page: () => BookingHistoryScreen()),
    GetPage(name: historyDetail, page: () => HistoryDetailScreen()),
  ];
}
