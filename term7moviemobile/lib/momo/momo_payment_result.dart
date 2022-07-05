class MomoPaymentResult {
  bool? isSuccess;
  int? status;
  String? token;
  String? phonenumber;

  MomoPaymentResult({
    this.isSuccess,
    this.status,
    this.token,
    this.phonenumber,
  });

  static MomoPaymentResult fromMap(Map<String, dynamic>? map) {
    return MomoPaymentResult(
      isSuccess: map?['isSuccess'] ?? false,
      status: map?['status'] ?? 0,
      token: map?['token'] ?? null,
      phonenumber: map?['phonenumber'] ?? null,
    );
  }
}
