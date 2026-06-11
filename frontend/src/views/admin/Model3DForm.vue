<template>
  <div class="max-w-4xl mx-auto space-y-6 pb-12">
    <!-- Back Header -->
    <div class="flex items-center space-x-3 bg-white p-5 rounded-2xl border border-slate-200/80 shadow-sm">
      <button 
        type="button"
        @click="goBack" 
        class="premium-btn p-2 hover:bg-slate-50 border border-slate-200 rounded-xl text-slate-500 hover:text-slate-800 transition"
        title="Quay lại"
      >
        <ChevronLeftIcon class="w-4 h-4" />
      </button>
      <div>
        <h2 class="text-base font-black text-slate-800 uppercase tracking-wide">
          {{ isEdit ? 'Chỉnh sửa mô hình 3D' : 'Thêm mô hình 3D mới' }}
        </h2>
        <p class="text-xs text-slate-450 font-medium">Thiết lập tệp tin `.glb` và mô tả cho mô hình du lịch.</p>
      </div>
    </div>

    <form @submit.prevent="submitForm" class="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
      <!-- Left Column: Form inputs (7 Columns) -->
      <div class="lg:col-span-7 space-y-6">
        <!-- Section 1: General Info -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-2.5">Thông tin chung</h3>
          
          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Tiêu đề mô hình *</label>
            <input 
              v-model="form.title"
              type="text"
              required
              placeholder="Ví dụ: Mô hình Nhà Cổ Bình Thủy..."
              class="w-full px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition font-medium"
            />
          </div>

          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Mô tả chi tiết</label>
            <textarea 
              v-model="form.description"
              rows="4"
              placeholder="Mô tả bổ sung về kiến trúc, lịch sử, các góc quan sát nổi bật của mô hình..."
              class="w-full px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition resize-none font-medium"
            ></textarea>
          </div>

          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Số lượng đa giác (Polygon Count)</label>
              <input 
                v-model.number="form.polygonCount"
                type="number"
                placeholder="Ví dụ: 25000 (không bắt buộc)"
                class="w-full px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition font-medium"
              />
            </div>
            <div>
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Định dạng file (Format)</label>
              <input 
                v-model="form.format"
                type="text"
                placeholder="Tự động tính từ file hoặc nhập glb/gltf"
                class="w-full px-3.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white focus:ring-4 focus:ring-teal-500/5 rounded-xl text-xs focus:outline-none transition font-medium"
              />
            </div>
          </div>
        </div>

        <!-- Section 2: File upload & selection -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-2.5">Tệp tin mô hình 3D (.glb) *</h3>
          
          <!-- Selected file preview/status -->
          <div v-if="form.mediaUrl" class="p-3.5 bg-teal-50/25 border border-teal-500/20 rounded-xl flex justify-between items-center text-xs">
            <div class="flex items-center space-x-2.5 overflow-hidden">
              <BoxIcon class="w-5 h-5 text-teal-600 flex-shrink-0" />
              <div class="overflow-hidden">
                <span class="font-bold text-slate-700 block truncate max-w-[220px]">{{ getFileNameFromUrl(form.mediaUrl) }}</span>
                <span class="text-[9px] font-mono text-slate-400 uppercase">Selected Model</span>
              </div>
            </div>
            <button 
              type="button" 
              @click="clearMedia"
              class="text-rose-500 hover:text-rose-600 text-[10px] font-bold uppercase transition"
            >
              Hủy chọn
            </button>
          </div>

          <!-- Tabs for source: Library vs Upload -->
          <div v-else class="space-y-4">
            <div class="flex border-b border-slate-150">
              <button 
                type="button"
                @click="modelSourceTab = 'upload'"
                :class="[
                  'px-4 py-2 text-xs font-bold transition-all border-b-2',
                  modelSourceTab === 'upload' ? 'border-teal-650 text-teal-700' : 'border-transparent text-slate-450 hover:text-slate-700'
                ]"
              >
                Tải file mới (.glb)
              </button>
              <button 
                type="button"
                @click="openMediaLibrary('model3d')"
                :class="[
                  'px-4 py-2 text-xs font-bold transition-all border-b-2',
                  modelSourceTab === 'library' ? 'border-teal-650 text-teal-700' : 'border-transparent text-slate-450 hover:text-slate-700'
                ]"
              >
                Chọn từ thư viện
              </button>
            </div>

            <!-- Upload Pane -->
            <div v-if="modelSourceTab === 'upload'" class="space-y-2">
              <div class="flex items-center justify-center w-full">
                <label class="flex flex-col items-center justify-center w-full h-36 border-2 border-dashed border-slate-200 rounded-2xl cursor-pointer hover:bg-slate-50/50 hover:border-teal-500/35 transition">
                  <div class="flex flex-col items-center justify-center pt-5 pb-6 space-y-1">
                    <UploadCloudIcon class="w-8 h-8 text-slate-400 mb-1" />
                    <p class="text-xs font-bold text-slate-600">Click để chọn tệp mô hình</p>
                    <p class="text-[10px] text-slate-400 font-semibold uppercase">Chỉ hỗ trợ .glb • Dung lượng tối đa 50MB</p>
                  </div>
                  <input 
                    type="file" 
                    accept=".glb" 
                    class="hidden" 
                    @change="handleFileUpload($event, 'model3d')"
                    :disabled="uploading"
                  />
                </label>
              </div>

              <!-- Upload progress -->
              <div v-if="uploading" class="flex items-center space-x-2 text-xs text-teal-600 font-semibold bg-teal-50/20 p-2.5 border border-teal-100 rounded-xl">
                <Loader2Icon class="w-4 h-4 animate-spin" />
                <span>Đang tải tệp lên máy chủ...</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Section 3: Thumbnail image -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-2.5">Hình ảnh đại diện (Thumbnail)</h3>
          
          <!-- Selected thumbnail status -->
          <div v-if="form.thumbnailUrl" class="p-3.5 bg-slate-50 border border-slate-150 rounded-xl flex justify-between items-center text-xs">
            <div class="flex items-center space-x-3 overflow-hidden">
              <img :src="form.thumbnailUrl" class="w-10 h-10 object-cover rounded-lg border border-slate-200 flex-shrink-0" />
              <div class="overflow-hidden">
                <span class="font-bold text-slate-700 block truncate max-w-[200px]">{{ getFileNameFromUrl(form.thumbnailUrl) }}</span>
                <span class="text-[9px] font-mono text-slate-400 uppercase">Selected Thumbnail</span>
              </div>
            </div>
            <button 
              type="button" 
              @click="clearThumbnail"
              class="text-rose-500 hover:text-rose-600 text-[10px] font-bold uppercase transition"
            >
              Hủy chọn
            </button>
          </div>

          <!-- Selection tabs -->
          <div v-else class="space-y-4">
            <div class="flex border-b border-slate-150">
              <button 
                type="button"
                @click="thumbSourceTab = 'upload'"
                :class="[
                  'px-4 py-2 text-xs font-bold transition-all border-b-2',
                  thumbSourceTab === 'upload' ? 'border-teal-650 text-teal-700' : 'border-transparent text-slate-450 hover:text-slate-700'
                ]"
              >
                Tải ảnh mới (.jpg/.png)
              </button>
              <button 
                type="button"
                @click="openMediaLibrary('image')"
                :class="[
                  'px-4 py-2 text-xs font-bold transition-all border-b-2',
                  thumbSourceTab === 'library' ? 'border-teal-650 text-teal-700' : 'border-transparent text-slate-450 hover:text-slate-700'
                ]"
              >
                Chọn từ thư viện
              </button>
            </div>

            <!-- Upload Pane -->
            <div v-if="thumbSourceTab === 'upload'" class="space-y-2">
              <div class="flex items-center justify-center w-full">
                <label class="flex flex-col items-center justify-center w-full h-28 border-2 border-dashed border-slate-200 rounded-2xl cursor-pointer hover:bg-slate-50/50 hover:border-teal-500/35 transition">
                  <div class="flex flex-col items-center justify-center pt-3 pb-3 space-y-1">
                    <ImageIcon class="w-6 h-6 text-slate-400 mb-0.5" />
                    <p class="text-xs font-bold text-slate-650">Click chọn ảnh đại diện</p>
                    <p class="text-[10px] text-slate-400 font-semibold uppercase">JPEG, PNG, WebP • Dưới 5MB</p>
                  </div>
                  <input 
                    type="file" 
                    accept="image/*" 
                    class="hidden" 
                    @change="handleFileUpload($event, 'image')"
                    :disabled="uploadingThumb"
                  />
                </label>
              </div>

              <!-- Uploading spinner -->
              <div v-if="uploadingThumb" class="flex items-center space-x-2 text-xs text-teal-600 font-semibold bg-teal-50/20 p-2.5 border border-teal-100 rounded-xl">
                <Loader2Icon class="w-4 h-4 animate-spin" />
                <span>Đang tải ảnh lên...</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Column: Settings & Live Preview (5 Columns) -->
      <div class="lg:col-span-5 space-y-6">
        <!-- Configuration Card -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-2.5">Cấu hình liên kết</h3>
          
          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Loại liên kết *</label>
            <select 
              v-model="form.targetType"
              required
              @change="onTargetTypeChange"
              class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
            >
              <option value="destination">Địa điểm du lịch (Destination)</option>
              <option value="panorama">Cảnh 360° (Panorama)</option>
              <option value="hotspot">Điểm tương tác (Hotspot)</option>
            </select>
          </div>

          <!-- If Destination -->
          <div v-if="form.targetType === 'destination'">
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Địa điểm liên kết *</label>
            <select 
              v-model="form.targetId"
              required
              class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
            >
              <option :value="null" disabled>Chọn địa điểm liên kết</option>
              <option 
                v-for="d in destinations" 
                :key="d.id" 
                :value="d.id"
              >
                {{ d.name }}
              </option>
            </select>
          </div>

          <!-- If Panorama or Hotspot (requires hierarchical selection) -->
          <div v-else class="space-y-4">
            <!-- 1. Destination Selection -->
            <div>
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Địa điểm du lịch *</label>
              <select 
                v-model="selectedDestinationId"
                required
                @change="onDestinationChange(selectedDestinationId)"
                class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
              >
                <option :value="null" disabled>Chọn địa điểm du lịch</option>
                <option 
                  v-for="d in destinations" 
                  :key="d.id" 
                  :value="d.id"
                >
                  {{ d.name }}
                </option>
              </select>
            </div>

            <!-- 2. Virtual Tour Selection -->
            <div v-if="selectedDestinationId">
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Tour ảo 360° *</label>
              <select 
                v-model="selectedTourId"
                required
                @change="onTourChange(selectedTourId)"
                class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
                :disabled="tours.length === 0"
              >
                <option :value="null" disabled>{{ tours.length === 0 ? 'Không có tour ảo nào' : 'Chọn tour ảo 360°' }}</option>
                <option 
                  v-for="t in tours" 
                  :key="t.id" 
                  :value="t.id"
                >
                  {{ t.title }}
                </option>
              </select>
            </div>

            <!-- 3. Panorama Selection -->
            <div v-if="selectedTourId">
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">
                {{ form.targetType === 'panorama' ? 'Cảnh 360° liên kết *' : 'Cảnh 360° *' }}
              </label>
              <select 
                v-if="form.targetType === 'panorama'"
                v-model="form.targetId"
                required
                class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
                :disabled="panoramas.length === 0"
              >
                <option :value="null" disabled>{{ panoramas.length === 0 ? 'Không có cảnh 360° nào' : 'Chọn cảnh 360°' }}</option>
                <option 
                  v-for="p in panoramas" 
                  :key="p.id" 
                  :value="p.id"
                >
                  {{ p.title }}
                </option>
              </select>

              <select 
                v-else
                v-model="selectedPanoramaId"
                required
                @change="onPanoramaChange(selectedPanoramaId)"
                class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
                :disabled="panoramas.length === 0"
              >
                <option :value="null" disabled>{{ panoramas.length === 0 ? 'Không có cảnh 360° nào' : 'Chọn cảnh 360°' }}</option>
                <option 
                  v-for="p in panoramas" 
                  :key="p.id" 
                  :value="p.id"
                >
                  {{ p.title }}
                </option>
              </select>
            </div>

            <!-- 4. Hotspot Selection -->
            <div v-if="form.targetType === 'hotspot' && selectedPanoramaId">
              <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Điểm tương tác Hotspot *</label>
              <select 
                v-model="form.targetId"
                required
                class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
                :disabled="hotspots.length === 0"
              >
                <option :value="null" disabled>{{ hotspots.length === 0 ? 'Không có hotspot Model 3D nào' : 'Chọn hotspot Model 3D' }}</option>
                <option 
                  v-for="h in hotspots" 
                  :key="h.id" 
                  :value="h.id"
                >
                  {{ h.title }}
                </option>
              </select>
              <p v-if="hotspots.length === 0 && panoramas.length > 0" class="text-[10px] text-amber-600 mt-1 font-medium">
                * Lưu ý: Cảnh này chưa được tạo Hotspot loại "Xem mô hình 3D" trong Virtual Tour. Vui lòng tạo Hotspot trước.
              </p>
            </div>
          </div>

          <div>
            <label class="block text-[10px] font-bold text-slate-500 uppercase tracking-wider mb-1">Trạng thái xuất bản *</label>
            <select 
              v-model="form.status"
              required
              class="w-full px-2.5 py-2.5 bg-slate-50 border border-slate-200 focus:border-teal-500/80 focus:bg-white rounded-xl text-xs focus:outline-none transition cursor-pointer appearance-none font-semibold"
            >
              <option value="draft">Bản nháp (Draft)</option>
              <option value="published">Đã xuất bản (Published)</option>
              <option value="archived">Lưu trữ (Archived)</option>
            </select>
          </div>
        </div>

        <!-- Live 3D Preview Box -->
        <div class="bg-white p-6 rounded-2xl border border-slate-200/80 shadow-sm space-y-4">
          <h3 class="text-xs font-black text-slate-400 uppercase tracking-wider border-b border-slate-100 pb-2.5">Trình xem thử 3D Live</h3>
          
          <div v-if="form.mediaUrl" class="h-80 w-full rounded-xl overflow-hidden border border-slate-150 relative">
            <ModelViewer :src="form.mediaUrl" />
          </div>

          <div v-else class="h-64 border-2 border-dashed border-slate-150 bg-slate-50/50 rounded-xl flex flex-col items-center justify-center text-center p-6 text-slate-400 space-y-2">
            <Rotate3dIcon class="w-8 h-8 stroke-[1.5]" />
            <p class="text-xs font-bold">Chưa nạp tệp mô hình</p>
            <p class="text-[9px] text-slate-400 leading-normal max-w-[180px] mx-auto">Vui lòng tải lên tệp `.glb` hoặc chọn từ thư viện để kích hoạt trình xem thử trực tiếp.</p>
          </div>
        </div>

        <!-- Action Button Deck -->
        <div class="flex items-center gap-3">
          <button 
            type="submit"
            class="premium-btn flex-grow py-3 bg-teal-700 hover:bg-teal-850 text-white font-extrabold rounded-xl shadow-md transition duration-150 flex items-center justify-center space-x-2 text-xs"
            :disabled="submitting"
          >
            <Loader2Icon v-if="submitting" class="w-4 h-4 animate-spin" />
            <span>{{ isEdit ? 'Lưu cập nhật' : 'Tạo mô hình 3D' }}</span>
          </button>
          
          <button 
            type="button"
            @click="goBack"
            class="premium-btn px-5 py-3 border border-slate-200 hover:bg-slate-50 text-slate-500 font-extrabold rounded-xl text-xs transition"
            :disabled="submitting"
          >
            Hủy
          </button>
        </div>
      </div>
    </form>

    <!-- Library Selector Modal Overlay -->
    <div 
      v-if="showLibraryModal" 
      class="fixed inset-0 z-50 bg-slate-900/60 backdrop-blur-sm flex items-center justify-center p-4"
      @click.self="closeLibraryModal"
    >
      <div class="bg-white rounded-3xl w-full max-w-2xl shadow-2xl border border-slate-150 overflow-hidden flex flex-col max-h-[80vh] animate-scale-up">
        <div class="p-6 border-b border-slate-100 flex justify-between items-center">
          <div>
            <h3 class="text-base font-black text-slate-800">Chọn tệp từ Thư viện</h3>
            <p class="text-xs text-slate-450 font-semibold">Chọn tệp thuộc định dạng: {{ libraryFilterType === 'model3d' ? '3D models (.glb)' : 'Hình ảnh (.jpg, .png)' }}</p>
          </div>
          <button 
            type="button" 
            @click="closeLibraryModal"
            class="p-2 hover:bg-slate-100 rounded-xl text-slate-400 hover:text-slate-650 transition"
          >
            <XIcon class="w-5 h-5" />
          </button>
        </div>

        <!-- Grid Body -->
        <div class="p-6 overflow-y-auto flex-1 bg-white min-h-[250px] custom-scrollbar">
          <div v-if="libraryLoading" class="grid grid-cols-3 sm:grid-cols-4 gap-4 animate-pulse">
            <div v-for="i in 8" :key="i" class="aspect-square bg-slate-100 rounded-xl"></div>
          </div>

          <div v-else-if="libraryMedia.length === 0" class="text-center py-16 space-y-3">
            <ImageIcon class="w-8 h-8 mx-auto text-slate-300" />
            <p class="text-sm font-semibold text-slate-500">Thư viện trống hoặc không có tệp nào phù hợp.</p>
          </div>

          <div v-else class="grid grid-cols-3 sm:grid-cols-4 gap-4">
            <div 
              v-for="img in libraryMedia" 
              :key="img.id"
              :class="[
                'aspect-square rounded-xl overflow-hidden cursor-pointer border-2 transition relative flex flex-col items-center justify-center p-2 text-center bg-slate-50',
                isMediaSelected(img) ? 'border-teal-500 shadow-md ring-2 ring-teal-500/20' : 'border-slate-100 hover:border-slate-350'
              ]"
              @click="selectMedia(img)"
            >
              <img v-if="img.mediaType === 'image'" :src="img.fileUrl" class="w-full h-full object-cover rounded-lg" />
              <div v-else class="space-y-2 text-slate-400">
                <BoxIcon class="w-8 h-8 mx-auto text-teal-600 stroke-[1.5]" />
                <span class="text-[9px] block truncate max-w-[120px] font-bold text-slate-600">{{ img.fileName }}</span>
              </div>

              <!-- Selection badge overlay -->
              <div 
                v-if="isMediaSelected(img)"
                class="absolute inset-0 bg-teal-500/10 flex items-center justify-center"
              >
                <div class="p-1 bg-teal-500 text-slate-950 rounded-full shadow-md">
                  <CheckIcon class="w-3.5 h-3.5 stroke-[2.5]" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Pagination -->
        <div 
          v-if="libraryMedia.length > 0 && libraryTotalPages > 1"
          class="p-4 bg-slate-50 border-t border-slate-100 flex justify-between items-center text-xs"
        >
          <span class="text-slate-550 font-bold">Trang {{ libraryPage }} / {{ libraryTotalPages }}</span>
          <div class="flex items-center space-x-2">
            <button 
              type="button" 
              @click="libraryPrevPage"
              :disabled="libraryPage === 1"
              class="premium-btn px-2.5 py-1.5 bg-white border border-slate-200 rounded-lg hover:bg-slate-50 disabled:opacity-40 transition shadow-sm"
            >
              Trước
            </button>
            <button 
              type="button" 
              @click="libraryNextPage"
              :disabled="libraryPage === libraryTotalPages"
              class="premium-btn px-2.5 py-1.5 bg-white border border-slate-200 rounded-lg hover:bg-slate-50 disabled:opacity-40 transition shadow-sm"
            >
              Sau
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../../utils/api'
import { useToastStore } from '../../stores/toast'
import ModelViewer from '../../components/public/ModelViewer.vue'
import { 
  ChevronLeft as ChevronLeftIcon, 
  Box as BoxIcon, 
  MapPin as MapPinIcon, 
  UploadCloud as UploadCloudIcon, 
  Loader2 as Loader2Icon, 
  Image as ImageIcon,
  Check as CheckIcon,
  X as XIcon,
  Rotate3d as Rotate3dIcon
} from 'lucide-vue-next'

const route = useRoute()
const router = useRouter()
const toastStore = useToastStore()

const isEdit = ref(false)
const submitting = ref(false)
const destinations = ref([])

const form = ref({
  title: '',
  description: '',
  mediaId: null,
  mediaUrl: null,
  thumbnailId: null,
  thumbnailUrl: null,
  targetType: 'destination',
  targetId: null,
  format: '',
  polygonCount: null,
  status: 'draft'
})

// Tab states
const modelSourceTab = ref('upload') // upload / library
const thumbSourceTab = ref('upload') // upload / library

// Upload states
const uploading = ref(false)
const uploadingThumb = ref(false)

// Library Modal states
const showLibraryModal = ref(false)
const libraryFilterType = ref('model3d') // model3d / image
const libraryMedia = ref([])
const libraryLoading = ref(false)
const libraryPage = ref(1)
const libraryTotalPages = ref(1)
const libraryPageSize = ref(12)

const fetchDestinations = async () => {
  try {
    const res = await api.get('/api/admin/destinations', {
      params: { page: 1, pageSize: 100 }
    })
    if (res.success) {
      destinations.value = res.data.items || []
    }
  } catch (err) {
    console.error('Failed to fetch destinations:', err)
  }
}

const tours = ref([])
const panoramas = ref([])
const hotspots = ref([])

const selectedDestinationId = ref(null)
const selectedTourId = ref(null)
const selectedPanoramaId = ref(null)
const loadingHierarchy = ref(false)

const onTargetTypeChange = () => {
  form.value.targetId = null
  selectedDestinationId.value = null
  selectedTourId.value = null
  selectedPanoramaId.value = null
  tours.value = []
  panoramas.value = []
  hotspots.value = []
}

const fetchToursForDestination = async (destId) => {
  try {
    const res = await api.get(`/api/admin/destinations/${destId}/virtual-tours`)
    if (res.success) {
      tours.value = res.data || []
    }
  } catch (err) {
    console.error('Failed to fetch tours:', err)
  }
}

const fetchPanoramasForTour = async (tourId) => {
  try {
    const res = await api.get(`/api/admin/virtual-tours/${tourId}/panoramas`)
    if (res.success) {
      panoramas.value = res.data || []
    }
  } catch (err) {
    console.error('Failed to fetch panoramas:', err)
  }
}

const fetchHotspotsForPanorama = async (panoId) => {
  try {
    const res = await api.get(`/api/admin/panoramas/${panoId}/hotspots`)
    if (res.success) {
      hotspots.value = (res.data || []).filter(h => h.type === 'model3d')
    }
  } catch (err) {
    console.error('Failed to fetch hotspots:', err)
  }
}

const onDestinationChange = async (destId) => {
  if (loadingHierarchy.value) return
  selectedTourId.value = null
  selectedPanoramaId.value = null
  tours.value = []
  panoramas.value = []
  hotspots.value = []
  
  if (form.value.targetType === 'destination') {
    form.value.targetId = destId
  } else {
    form.value.targetId = null
  }
  
  if (destId) {
    await fetchToursForDestination(destId)
  }
}

const onTourChange = async (tourId) => {
  if (loadingHierarchy.value) return
  selectedPanoramaId.value = null
  panoramas.value = []
  hotspots.value = []
  form.value.targetId = null
  
  if (tourId) {
    await fetchPanoramasForTour(tourId)
  }
}

const onPanoramaChange = async (panoId) => {
  if (loadingHierarchy.value) return
  hotspots.value = []
  
  if (form.value.targetType === 'panorama') {
    form.value.targetId = panoId
  } else {
    form.value.targetId = null
    if (panoId) {
      await fetchHotspotsForPanorama(panoId)
    }
  }
}

const loadHierarchyForEdit = async (targetType, targetId) => {
  loadingHierarchy.value = true
  try {
    if (targetType === 'destination') {
      selectedDestinationId.value = targetId
    } else if (targetType === 'panorama') {
      const panoRes = await api.get(`/api/admin/panoramas/${targetId}`)
      if (panoRes.success) {
        const pano = panoRes.data
        const tourRes = await api.get(`/api/admin/virtual-tours/${pano.virtualTourId}`)
        if (tourRes.success) {
          const tour = tourRes.data
          selectedDestinationId.value = tour.destinationId
          selectedTourId.value = pano.virtualTourId
          
          await fetchToursForDestination(tour.destinationId)
          await fetchPanoramasForTour(pano.virtualTourId)
        }
      }
    } else if (targetType === 'hotspot') {
      const hotspotRes = await api.get(`/api/admin/hotspots/${targetId}`)
      if (hotspotRes.success) {
        const hotspot = hotspotRes.data
        const panoRes = await api.get(`/api/admin/panoramas/${hotspot.panoramaId}`)
        if (panoRes.success) {
          const pano = panoRes.data
          const tourRes = await api.get(`/api/admin/virtual-tours/${pano.virtualTourId}`)
          if (tourRes.success) {
            const tour = tourRes.data
            selectedDestinationId.value = tour.destinationId
            selectedTourId.value = pano.virtualTourId
            selectedPanoramaId.value = hotspot.panoramaId
            
            await fetchToursForDestination(tour.destinationId)
            await fetchPanoramasForTour(pano.virtualTourId)
            await fetchHotspotsForPanorama(hotspot.panoramaId)
          }
        }
      }
    }
  } catch (err) {
    console.error('Failed to load target hierarchy:', err)
  } finally {
    loadingHierarchy.value = false
  }
}

const fetchModelDetail = async (id) => {
  try {
    const res = await api.get(`/api/admin/models-3d/${id}`)
    if (res.success) {
      const d = res.data
      form.value = {
        title: d.title,
        description: d.description || '',
        mediaId: d.mediaId,
        mediaUrl: d.modelUrl,
        thumbnailId: d.thumbnailId,
        thumbnailUrl: d.thumbnailUrl,
        targetType: d.targetType,
        targetId: d.targetId,
        format: d.format || '',
        polygonCount: d.polygonCount,
        status: d.status
      }
      
      // Load target hierarchy path for edit mode
      await loadHierarchyForEdit(d.targetType, d.targetId)
      
      // Default to library source selection if data already present
      modelSourceTab.value = 'library'
      if (d.thumbnailId) {
        thumbSourceTab.value = 'library'
      }
    }
  } catch (err) {
    toastStore.error(err.message || 'Không thể tải chi tiết mô hình 3D.')
    router.push('/admin/models-3d')
  }
}

// Media Selection Library
const openMediaLibrary = async (type) => {
  libraryFilterType.value = type
  if (type === 'model3d') {
    modelSourceTab.value = 'library'
  } else {
    thumbSourceTab.value = 'library'
  }
  libraryPage.value = 1
  showLibraryModal.value = true
  await fetchLibraryMedia()
}

const closeLibraryModal = () => {
  showLibraryModal.value = false
}

const fetchLibraryMedia = async () => {
  libraryLoading.value = true
  try {
    const res = await api.get('/api/admin/media', {
      params: {
        mediaType: libraryFilterType.value,
        page: libraryPage.value,
        pageSize: libraryPageSize.value
      }
    })
    if (res.success) {
      libraryMedia.value = res.data.items || []
      libraryPage.value = res.data.page
      libraryTotalPages.value = res.data.totalPages
    }
  } catch (err) {
    console.error('Failed to load library media:', err)
  } finally {
    libraryLoading.value = false
  }
}

const selectMedia = (media) => {
  if (libraryFilterType.value === 'model3d') {
    form.value.mediaId = media.id
    form.value.mediaUrl = media.fileUrl
    if (!form.value.format && media.extension) {
      form.value.format = media.extension.replace('.', '').toLowerCase()
    }
  } else {
    form.value.thumbnailId = media.id
    form.value.thumbnailUrl = media.fileUrl
  }
  closeLibraryModal()
}

const isMediaSelected = (media) => {
  if (libraryFilterType.value === 'model3d') {
    return form.value.mediaId === media.id
  }
  return form.value.thumbnailId === media.id
}

const libraryPrevPage = async () => {
  if (libraryPage.value > 1) {
    libraryPage.value--
    await fetchLibraryMedia()
  }
}

const libraryNextPage = async () => {
  if (libraryPage.value < libraryTotalPages.value) {
    libraryPage.value++
    await fetchLibraryMedia()
  }
}

// Direct file uploading
const handleFileUpload = async (event, type) => {
  const file = event.target.files[0]
  if (!file) return

  // Basic uploader constraints
  if (type === 'model3d') {
    if (file.size > 50 * 1024 * 1024) {
      toastStore.error('Dung lượng tệp mô hình 3D vượt quá giới hạn cho phép (50MB).')
      return
    }
    uploading.value = true
  } else {
    if (file.size > 5 * 1024 * 1024) {
      toastStore.error('Dung lượng hình ảnh đại diện vượt quá giới hạn (5MB).')
      return
    }
    uploadingThumb.value = true
  }

  try {
    const formData = new FormData()
    formData.append('file', file)
    formData.append('mediaType', type)

    const res = await api.post('/api/admin/media/upload', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })

    if (res.success) {
      toastStore.success('Tải tệp tin lên thư viện thành công!')
      if (type === 'model3d') {
        form.value.mediaId = res.data.id
        form.value.mediaUrl = res.data.fileUrl
        form.value.format = res.data.extension.replace('.', '').toLowerCase()
      } else {
        form.value.thumbnailId = res.data.id
        form.value.thumbnailUrl = res.data.fileUrl
      }
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi xảy ra khi tải tệp tin lên server.')
  } finally {
    uploading.value = false
    uploadingThumb.value = false
  }
}

// File cleaners
const clearMedia = () => {
  form.value.mediaId = null
  form.value.mediaUrl = null
  modelSourceTab.value = 'upload'
}

const clearThumbnail = () => {
  form.value.thumbnailId = null
  form.value.thumbnailUrl = null
  thumbSourceTab.value = 'upload'
}

const submitForm = async () => {
  if (!form.value.mediaId) {
    toastStore.error('Vui lòng tải lên hoặc chọn một tệp mô hình 3D.')
    return
  }
  if (!form.value.targetId) {
    toastStore.error('Vui lòng chọn đối tượng liên kết.')
    return
  }

  submitting.value = true
  try {
    const payload = {
      title: form.value.title,
      description: form.value.description,
      mediaId: form.value.mediaId,
      thumbnailId: form.value.thumbnailId,
      targetType: form.value.targetType,
      targetId: form.value.targetId,
      format: form.value.format || null,
      polygonCount: form.value.polygonCount || null,
      status: form.value.status
    }

    let res
    if (isEdit.value) {
      res = await api.put(`/api/admin/models-3d/${route.params.id}`, payload)
    } else {
      res = await api.post('/api/admin/models-3d', payload)
    }

    if (res.success) {
      toastStore.success(isEdit.value ? 'Cập nhật mô hình 3D thành công!' : 'Tạo mới mô hình 3D thành công!')
      router.push('/admin/models-3d')
    }
  } catch (err) {
    toastStore.error(err.message || 'Lỗi xảy ra khi lưu thông tin mô hình.')
  } finally {
    submitting.value = false
  }
}

// Helpers
const getFileNameFromUrl = (url) => {
  if (!url) return ''
  return url.substring(url.lastIndexOf('/') + 1)
}

const goBack = () => {
  router.push('/admin/models-3d')
}

onMounted(async () => {
  await fetchDestinations()
  const id = route.params.id
  if (id) {
    isEdit.value = true
    await fetchModelDetail(id)
  }
})
</script>
