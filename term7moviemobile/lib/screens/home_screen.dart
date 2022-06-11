import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/home_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/carousel/carousel_data_found.dart';
import 'package:term7moviemobile/widgets/carousel/carousel_loading.dart';
import 'package:cached_network_image/cached_network_image.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/widgets/bottom_bar.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:term7moviemobile/widgets/movie_item.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key}) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    SystemChrome.setSystemUIOverlayStyle(
        const SystemUiOverlayStyle(statusBarColor: Colors.transparent));
    return SafeArea(
      child: Scaffold(
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
                  GestureDetector(
                    onTap: () {
                      // Get.to(() => const SelectionLocationScreen());
                    },
                    child: Row(
                      children: [
                        Icon(Icons.add_location_alt_rounded,
                            color: MyTheme.bottomBarColor, size: 16),
                        // Obx(
                        //       () => Text(
                        //     //LocationController.instance.city.value,
                        //         'ho chi minh city',
                        //     style: TextStyle(color: MyTheme.grayColor, inherit: true, fontSize: 8),
                        //   ),
                        // ),
                        Text(
                          //LocationController.instance.city.value,
                          'ho chi minh city',
                          style: TextStyle(
                              color: MyTheme.grayColor,
                              inherit: true,
                              fontSize: 14),
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
        body: Container(
          height: size.height,
          width: size.width,
          child: SingleChildScrollView(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              children: [
                GetBuilder<HomeController>(
                  builder: (_c) {
                    if (_c.isLoading) if (_c.movies.length > 0)
                      return CarouselSliderDataFound(carouselList: _c.movies);
                    else
                      return CarouselLoading();
                    else if (_c.movies.length > 0)
                      return CarouselSliderDataFound(carouselList: _c.movies);
                    else
                      return Container();
                  },
                ),
                Padding(
                  padding: const EdgeInsets.only(left: 12, top: 10, right: 12),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        "Movies on Sales".toUpperCase(),
                        style: TextStyle(
                            fontWeight: FontWeight.bold,
                            color: Colors.black.withOpacity(0.8)),
                      ),
                      TextButton(
                        onPressed: () {},
                        child: const Text(
                          "View All",
                          style: TextStyle(color: MyTheme.primaryColor),
                        ),
                      ),
                    ],
                  ),
                ),
                Container(
                  height: 320,
                  width: size.width,
                  child: ListView.builder(
                    scrollDirection: Axis.horizontal,
                    itemCount: 3,
                    itemBuilder: (_, i) {
                      return Hero(
                        tag: "movie title",
                        child: MovieItem(),
                      );
                    },
                  ),
                ),

                ElevatedButton(
                  child: Center(
                    child: Text('Logout'),
                  ),
                  onPressed: () {
                    AuthController.instance.signOut();
                  },
                ),
              ],
            ),
          ),
        ),
        bottomNavigationBar: BottomBar(),
      ),
    );
  }
}
