#include <cstdint>
#include <type_traits>

#pragma once

namespace Core {
    using u16 = uint16_t;
    using u32 = uint32_t;
    using u64 = uint64_t;
        
    template <
        typename T, typename M,
        typename = std::enable_if<std::is_unsigned<T>::value>,
        typename = std::enable_if<std::is_enum<M>::value>
    >
    class Register{
    public:
        constexpr Register() = delete;
        constexpr Register<T, M>& operator&=(M mask) {
            _value &= (T) mask;
            return *this;
        }
        constexpr Register<T, M>& operator|=(M mask) {
            _value |= (T)mask;
            return *this;
        }
    private:
        volatile T _value;
    };
}
