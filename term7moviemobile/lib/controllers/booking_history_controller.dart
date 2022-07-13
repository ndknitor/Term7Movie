import 'package:get/get.dart';
import 'package:term7moviemobile/models/transaction_model.dart';
import 'package:term7moviemobile/services/transaction_services.dart';

class BookingHistoryController extends GetxController {
  static BookingHistoryController instance = Get.find();
  RxBool isLoading = false.obs;
  List<TransactionModel> transactions = [];
  var page = 1;

  void fetchData() async {
    try{
      isLoading.value = true;
      transactions = await TransactionServices.getTransactions({
        'page': page,
      });
      print(transactions);
    }finally{
      isLoading.value = false;
    }
  }
}