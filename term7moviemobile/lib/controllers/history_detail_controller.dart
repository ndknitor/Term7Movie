import 'package:get/get.dart';
import 'package:term7moviemobile/models/transaction_model.dart';
import 'package:term7moviemobile/services/transaction_services.dart';

class HistoryDetailController extends GetxController {
  static HistoryDetailController instance = Get.find();
  var isLoading = false.obs;
  TransactionModel? transaction;
  String id = Get.parameters['id']!;

  // @override
  // void onInit() {
  //   super.onInit();
  //   id = Get.parameters['id']!;
  //   fetchTransactionDetail();
  // }


  void fetchTransactionDetail() async {
    try {
      isLoading.value = true;
      print(id);
      transaction = await TransactionServices.getTransactionById(id);
    } finally {
      isLoading.value = false;
    }
  }
}
