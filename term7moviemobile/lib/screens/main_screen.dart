import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/location_controller.dart';
import 'package:term7moviemobile/controllers/main_controller.dart';
import 'package:term7moviemobile/screens/home_screen.dart';
import 'package:term7moviemobile/screens/movie_list_screen.dart';
import 'package:term7moviemobile/screens/profile_screen.dart';
import 'package:term7moviemobile/screens/showtime_screen.dart';
import 'package:term7moviemobile/utils/constants.dart';
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
          appBar: PreferredSize(
            preferredSize: const Size.fromHeight(80),
            child: AppBar(
              elevation: 0,
              backgroundColor: MyTheme.backgroundColor,
              leading: Padding(
                padding: const EdgeInsets.only(left: 12, top: 12),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(30),
                  child: CachedNetworkImage(
                    fit: BoxFit.cover,
                    imageUrl: AuthController.instance.user!.photoURL ??
                        Constants.defaultAvatar,
                  ),
                ),
              ),
              title: Padding(
                padding: const EdgeInsets.only(top: 12),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      AuthController.instance.user!.displayName ?? "Name",
                      style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.w600,
                          color: MyTheme.textColor),
                    ),
                    SizedBox(height: 2),
                    GestureDetector(
                      onTap: () {
                        Get.toNamed("/location");
                      },
                      child: Row(
                        children: [
                          Icon(Icons.add_location_alt_rounded,
                              color: MyTheme.bottomBarColor, size: 16),
                          Obx(
                                () => SizedBox(
                                  width: 190,
                                  child: Text(
                              LocationController.instance.city.value, maxLines: 1, overflow: TextOverflow.ellipsis,
                              style: TextStyle(color: MyTheme.grayColor, inherit: true, fontSize: 10),
                            ),
                                ),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
              actions: [
                Padding(
                  padding: const EdgeInsets.only(right: 12, top: 12),
                  child: IconButton(
                    onPressed: () {},
                    icon: SvgPicture.asset("assets/images/notifications.svg",
                        color: MyTheme.bottomBarColor),
                  ),
                ),
              ],
            ),
          ),
          backgroundColor: MyTheme.backgroundColor,
          body: IndexedStack(
            index: controller.index,
            children: [
              HomeScreen(),
              MovieListScreen(),
              ShowTimeScreen(),
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
                label: 'Movies',
                icon: Icon(Icons.movie_filter),
              ),
              BottomNavigationBarItem(
                label: 'Showtime',
                icon: Icon(Icons.local_movies_rounded),
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
