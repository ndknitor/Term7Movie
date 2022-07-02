import 'package:term7moviemobile/models/company_model.dart';
import 'package:term7moviemobile/services/api.dart';

class CompanyServices {
  static Future<List<CompanyModel>> getCompanies(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/companies', queryParameters: queryParams);
    return List<CompanyModel>.from(res.data['companies']['results'].map((x) => CompanyModel.fromJson(x)));
  }
}