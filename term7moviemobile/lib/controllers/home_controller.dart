import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:term7moviemobile/models/home_showtime_model.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';


class HomeController extends GetxController {
  var isLoading = false.obs;
  List<MovieModel> sliders = [];
  static HomeController instance = Get.find();
  List<HomeShowTimeModel> showtime = [];
  static late SharedPreferences pref;

  void fetchData() async {
    try{
      isLoading.value = true;
      List<MovieModel> _sliders = await MovieServices.getLatestMovies({'Action': 'latest'});
      sliders.assignAll(_sliders);
      pref = await SharedPreferences.getInstance();
      double longitude = double.parse(pref.getString("longitude")!);
      double latitude = double.parse(pref.getString("latitude")!);
      print(longitude);
      print(latitude);
      List<HomeShowTimeModel> _data = await MovieServices.getMovieShowTimeForHome({'Action': 'home',
        'Longitude': longitude,
        'Latitude': latitude,
      });
      showtime.assignAll(_data);
      print(showtime);
    } finally{
      isLoading.value = false;
    }
  }
}