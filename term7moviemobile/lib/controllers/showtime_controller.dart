import 'package:get/get.dart';
import 'package:term7moviemobile/models/company_model.dart';
import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/services/company_services.dart';
import 'package:term7moviemobile/services/showtime_services.dart';

class ShowtimeController extends GetxController {
  var isLoading = false.obs;
  List<CompanyModel> companies = [];
  List<ShowtimeModel> showtimes = [];
  var isSelected = (-1).obs;
  var theaterId = (-1).obs;

  @override
  void onInit() {
    super.onInit();
  }

  void fetchCompanies() async {
    try{
      isLoading.value = true;
      List<CompanyModel> _data = await CompanyServices.getCompanies({'TheaterIncluded': true});
      companies.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }

  void fetchShowtimes(int id) async {
    try{
      isLoading.value = true;
      List<ShowtimeModel> _data = await ShowtimeServices.getShowtimes({'TheaterId': id});
      print(_data);
      showtimes.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }
}