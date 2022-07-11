import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/home_controller.dart';
import 'package:term7moviemobile/controllers/location_controller.dart';
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

  @override
  void initState() {
    super.initState();
    controller.fetchMoviesForSlider();
    Get.put(LocationController());
    Get.find<LocationController>().getMyLocation().then((value) {
      controller.fetchTopMovies();
    });
  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    SystemChrome.setSystemUIOverlayStyle(
        const SystemUiOverlayStyle(statusBarColor: Colors.transparent));
    return Scaffold(
      body: SafeArea(
        child: Container(
            height: size.height,
            width: size.width,
            child: SingleChildScrollView(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  GetBuilder<HomeController>(
                    builder: (_c) {
                      if (_c.isLoading.value) if (_c.sliders.length > 0)
                        return CarouselSliderDataFound(carouselList: _c.sliders);
                      else
                        return CarouselLoading();
                      else if (_c.sliders.length > 0)
                        return CarouselSliderDataFound(carouselList: _c.sliders);
                      else
                        return Container();
                    },
                  ),
                  Padding(
                    padding: const EdgeInsets.only(left: 12, top: 10, right: 12, bottom: 10),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          "Showtime on Sales".toUpperCase(),
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
                      opacity: 0.1,
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
                    )),
                  ),
                ],
              ),
            ),
          ),
      ),
    );
  }
}


