import 'package:get/get.dart';
import 'package:term7moviemobile/models/company_model.dart';
import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/services/company_services.dart';
import 'package:term7moviemobile/services/showtime_services.dart';

class ShowtimeController extends GetxController {
  static ShowtimeController instance = Get.find();
  var isLoading = false.obs;
  List<CompanyModel> companies = [];
  List<ShowtimeModel> showtimes = [];
  var isSelected = (-1).obs;
  var theaterId = (-1).obs;
  var movieId = "0";
  DateTime seletedDate = DateTime.now();

  void fetchCompanies() async {
    try{
      isLoading.value = true;
      List<CompanyModel> _data = await CompanyServices.getCompanies({'TheaterIncluded': true});
      companies.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }

  void fetchShowtimes() async {
    // print({
    //   'TheaterId': theaterId,
    //   'MovieId': MovieDetailController.instance.id,
    //   'Date': seletedDate.toIso8601String().split('T')[0],
    // });
    try{
      isLoading.value = true;
      showtimes = [];
      List<ShowtimeModel> _data = await ShowtimeServices.getShowtimes({
        'theaterId': theaterId,
        'movieId': movieId,
        'date': seletedDate.toIso8601String().split('T')[0],
      });
      print(_data);
      showtimes.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }

  void handleDateChange(DateTime date) {
    seletedDate = date;
    isSelected.value = -1;
    theaterId.value = -1;
  }
}