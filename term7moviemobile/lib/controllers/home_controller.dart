import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:term7moviemobile/controllers/location_controller.dart';
import 'package:term7moviemobile/models/home_showtime_model.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';


class HomeController extends GetxController {
  var isLoading = false.obs;
  List<MovieModel> sliders = [];
  static HomeController instance = Get.find();
  RxList showtime = [].obs;
  static late SharedPreferences pref;

  @override
  void onInit() {
    Get.put(LocationController());
    fetchMoviesForSlider();
    Get.find<LocationController>().getMyLocation().then((value) {
      fetchTopMovies();
    });
    super.onInit();
  }

  void fetchMoviesForSlider() async {
    try{
      isLoading.value = true;
      List<MovieModel> _data = await MovieServices.getLatestMovies({'Action': 'latest'});
      sliders.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }

  void fetchTopMovies() async {
    try{
      isLoading.value = true;
      pref = await SharedPreferences.getInstance();
      double longitude = double.parse(pref.getString("longitude")!);
      double latitude = double.parse(pref.getString("latitude")!);
      print(longitude);
      print(latitude);
      List<HomeShowTimeModel> _data = await MovieServices.getMovieShowTimeForHome({'Action': 'home',
        'Longitude': longitude,
        'Latitude': latitude,
      });
      showtime = _data.obs;
      update();
      // print(showtime);
    }finally{
      isLoading.value = false;
    }
  }
}