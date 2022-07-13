import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/home_controller.dart';
import 'package:term7moviemobile/controllers/location_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/carousel/carousel_data_found.dart';
import 'package:term7moviemobile/widgets/carousel/carousel_loading.dart';
import 'package:term7moviemobile/widgets/card/movie_item.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({Key? key}) : super(key: key);

  @override
  State<HomeScreen> createState() => _HomeScreenState();

}

class _HomeScreenState extends State<HomeScreen> {
  HomeController controller = Get.put(HomeController());
  LocationController locationController = Get.put(LocationController());

  @override
  void initState() {
    super.initState();
    locationController.getMyLocation().then((value) {
      controller.fetchData();
    });
  }

  @override
  void dispose() {

  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    SystemChrome.setSystemUIOverlayStyle(
        const SystemUiOverlayStyle(statusBarColor: Colors.transparent));
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
                      fontWeight: FontWeight.w500,
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
            // Padding(
            //   padding: const EdgeInsets.only(right: 12, top: 12),
            //   child: IconButton(
            //     onPressed: () {},
            //     icon: SvgPicture.asset("assets/images/notifications.svg",
            //         color: MyTheme.bottomBarColor),
            //   ),
            // ),
          ],
        ),
      ),
      body: SafeArea(
        child: Container(
            height: size.height,
            width: size.width,
            child: SingleChildScrollView(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  Obx(() => !controller.isLoading.value ? CarouselSliderDataFound(carouselList: controller.sliders) : CarouselLoading()),
                  Padding(
                    padding: const EdgeInsets.only(left: 12, top: 10, right: 12, bottom: 10),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          "recommended showtime".toUpperCase(),
                          style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: Colors.black.withOpacity(0.8)),
                        ),
                      ],
                    ),
                  ),
                  Container(
                    height: 300,
                    width: size.width,
                    child: Obx(() => LoadingOverlay(
                      isLoading: controller.isLoading.value,
                      color: MyTheme.backgroundColor,
                      opacity: 1,
                      progressIndicator: const CircularProgressIndicator(
                        valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
                      ),
                      child: ListView.builder(
                          scrollDirection: Axis.horizontal,
                          itemCount: controller.showtime.length,
                          itemBuilder: (_, i) {
                            return MovieItem(data: controller.showtime[i]);
                          },
                        ),
                    ),
                    ),
                  ),
                ],
              ),
            ),
          ),
      ),
    );
  }
}


