//package com.example.term7moviemobile
//
//import android.annotation.TargetApi
//import android.app.Activity
//import android.content.Intent
//import android.os.Build
//import io.flutter.plugin.common.MethodChannel.Result
//import io.flutter.plugin.common.PluginRegistry
//import io.flutter.plugin.common.PluginRegistry.ActivityResultListener
//import vn.momo.momo_partner.AppMoMoLib
//import 	java.util.Base64
//
//class MomoVnPluginDelegate(private var registrar: PluginRegistry.Registrar? = null) : ActivityResultListener {
//
//    private var pendingResult: Result? = null
//    private var pendingReply: Map<String, Any>? = null
//
//    fun openCheckout(momoRequestPaymentData: Any, result: Result) {
//        this.pendingResult = result;
//        AppMoMoLib.getInstance().setAction(AppMoMoLib.ACTION.PAYMENT)
//        AppMoMoLib.getInstance().setActionType(AppMoMoLib.ACTION_TYPE.GET_TOKEN)
//
//        val paymentInfo: HashMap<String, Any> = momoRequestPaymentData as HashMap<String, Any>
//        val isTestMode: Boolean? = paymentInfo["isTestMode"] as Boolean?
//
//        if (isTestMode == null || !isTestMode) {
//            AppMoMoLib.getInstance().setEnvironment(AppMoMoLib.ENVIRONMENT.PRODUCTION)
//        } else {
//            AppMoMoLib.getInstance().setEnvironment(AppMoMoLib.ENVIRONMENT.DEVELOPMENT)
//        }
//
//        AppMoMoLib.getInstance().requestMoMoCallBack(registrar?.activity(), paymentInfo)
//    }
//
//    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?): Boolean {
//        if (resultCode == Activity.RESULT_OK) {
//            if (requestCode == AppMoMoLib.getInstance().REQUEST_CODE_MOMO) {
//                _handleResult(data)
//            }
//        }
//        return true
//    }
//    private fun sendReply(data: Map<String, Any>) {
//        if (this.pendingResult != null) {
//            this.pendingResult?.success(data)
//            pendingReply = null
//        } else {
//            pendingReply = data
//        }
//    }
//
//    private fun _handleResult(data: Intent?) {
//        data?.let {
//            val status = data.getIntExtra("status", -1)
//            var isSuccess: Boolean = false
//            if (status == MomoVnConfig.CODE_PAYMENT_SUCCESS) isSuccess = true
//            val token = data.getStringExtra("data").orEmpty()
//            val phonenumber = data.getStringExtra("phonenumber").orEmpty()
//            val message = data.getStringExtra("message").orEmpty()
//            val extra = data.getStringExtra("extra")
//            val data: MutableMap<String, Any> = java.util.HashMap()
//            data.put("isSuccess", isSuccess)
//            data.put("status", status)
//            data.put("phonenumber", phonenumber)
//            data.put("token", token)
//            data.put("message", message)
//            var decoded = ""
//            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
//                decoded = String(Base64.getMimeDecoder().decode(extra), Charsets.UTF_8)
//            } else if  (Build.VERSION.SDK_INT >= 26) {
//                decoded = android.util.Base64.decode(extra, android.util.Base64.DEFAULT) as String;
//            }
//            data.put("extra", decoded)
//            sendReply(data)
//        } ?: run {
//            val data: MutableMap<String, Any> = java.util.HashMap()
//            data.put("isSuccess", false)
//            data.put("status", 7);
//            data.put("phonenumber", "")
//            data.put("token", "")
//            data.put("message", "")
//            data.put("extra", "")
//            sendReply(data)
//        }
//    }
//
//}