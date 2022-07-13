import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/main_controller.dart';
import 'package:term7moviemobile/screens/home_screen.dart';
import 'package:term7moviemobile/screens/movie_list_screen.dart';
import 'package:term7moviemobile/screens/profile_screen.dart';
import 'package:term7moviemobile/screens/sale_tickets_screen.dart';
import 'package:term7moviemobile/screens/showtime_screen.dart';
import 'package:term7moviemobile/utils/theme.dart';

class MainScreen extends StatelessWidget {
  const MainScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    SystemChrome.setSystemUIOverlayStyle(
        const SystemUiOverlayStyle(statusBarColor: Colors.transparent));
    return GetBuilder<MainController>(
      builder: (controller) {
        return Scaffold(
          backgroundColor: MyTheme.backgroundColor,
          body: IndexedStack(
            index: controller.index,
            children: [
              HomeScreen(),
              SaleTicketsScreen(),
              MovieListScreen(),
              ProfileScreen(),
            ],
          ),
          bottomNavigationBar: BottomNavigationBar(
            type: BottomNavigationBarType.fixed,
            backgroundColor: Colors.white,
            selectedItemColor: MyTheme.primaryColor,
            unselectedItemColor: MyTheme.bottomBarColor,
            currentIndex: controller.index,
            selectedFontSize: 14,
            unselectedFontSize: 0,
            onTap: controller.changeIndex,
            items: [
              BottomNavigationBarItem(
                label: 'Home',
                icon: Icon(Icons.home_rounded),
              ),
              BottomNavigationBarItem(
                label: 'Sale Tickets',
                icon: Icon(Icons.local_activity_rounded),
              ),
              BottomNavigationBarItem(
                label: 'Movies',
                icon: Icon(Icons.movie_filter),
              ),
              BottomNavigationBarItem(
                label: 'Profile',
                icon: Icon(Icons.account_circle_rounded),
              ),
            ],
          ),
        );
      },
    );
  }
}
